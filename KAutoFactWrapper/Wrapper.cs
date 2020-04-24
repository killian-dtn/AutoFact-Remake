using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KAutoFactWrapper.Attributes;
using KAutoFactWrapper.Exceptions;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace KAutoFactWrapper
{
    public class Wrapper
    {
        private DbConnection Connection;
        private MySqlCompiler Compiler;

        /// <summary>
        /// Noms des tables (selon les attributs donnés aux Types) classés par Type leur type assigné.
        /// </summary>
        public Dictionary<Type, string> TableByClass { get; private set; }
        /// <summary>
        /// Types classés par nom des tables donnés dans leur attribut.
        /// </summary>
        public Dictionary<string, Type> ClassByTable { get; private set; }
        public Dictionary<string, Dictionary<string, PropertyInfo>> TableStructs { get; private set; }

        private static Wrapper instance = null;
        /// <summary>
        /// Instance du singleton.
        /// </summary>
        public static Wrapper Instance
        {
            get
            {
                if (Wrapper.instance == null)
                    Wrapper.instance = new Wrapper();
                return Wrapper.instance;
            }
        }

        private Wrapper()
        {
            this.Connection = DbConnection.Instance;
            this.Compiler = new MySqlCompiler();
            this.TableByClass = new Dictionary<Type, string>();
            this.ClassByTable = new Dictionary<string, Type>();
            this.TableStructs = new Dictionary<string, Dictionary<string, PropertyInfo>>();
            this.Load();
            this.LoadForeignKeys();
        }

        private void Load()
        {
            foreach(Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a.GetCustomAttribute(typeof(WrapperAttribute)) != null)
                {
                    foreach(Type t in a.GetTypes())
                    {
                        DbClassAttribute dca = null;
                        if (!Wrapper.IsQueryAble(t, ref dca))
                            continue;

                        bool HasPrimaryKey = false;
                        List<PropertyInfo> PrimaryProps = new List<PropertyInfo>();
                        Dictionary<string, PropertyInfo> TableStructTmp = new Dictionary<string, PropertyInfo>();
                        foreach(PropertyInfo prop in t.GetProperties())
                        {
                            DbPropAttribute dpa = null;
                            if (!Wrapper.IsQueryAble(prop, ref dpa) || string.IsNullOrEmpty(dpa.DbName))
                                continue;

                            if (dpa is IPrimaryKeyPropAttribute)
                            {
                                PrimaryProps.Add(prop);
                                HasPrimaryKey |= true;
                            }

                            if ((dpa is IForeignKeyPropAttribute || dpa is IPrimaryKeyPropAttribute) && string.IsNullOrEmpty(dpa.DbName))
                                throw new DbPropAttributeException();

                            try { TableStructTmp.Add(dpa.DbName, prop); }
                            catch(ArgumentException e) { throw new DbPropAttributeException($"La valeur DbName de l'attribut {typeof(DbPropAttribute).FullName} \"{dpa.DbName}\" est utilisée plusieurs fois dans la classe {t.FullName}.", e); }
                        }

                        if (!HasPrimaryKey)
                            throw new DbPropAttributeException($"Le type {t.FullName} n'a pas de clé primaire.");

                        dca.PrimaryKey = new PrimaryKeyStruct(t, PrimaryProps.ToArray());

                        this.TableByClass.Add(t, dca.DbName);

                        try
                        {
                            this.ClassByTable.Add(dca.DbName, t);
                            this.TableStructs.Add(dca.DbName, TableStructTmp);
                        }
                        catch (ArgumentNullException e) { throw new DbClassAttributeException($"La valeur DbName de l'attribut {typeof(DbClassAttribute).FullName} sur le type {t.FullName} est nulle.", e); }
                        catch (ArgumentException e) { throw new DbClassAttributeException($"La valeur DbName de l'attribut {typeof(DbClassAttribute).FullName} \"{dca.DbName}\" est utilisée sur plusieurs classes.", e); }
                    }
                }
            }
        }

        private void LoadForeignKeys()
        {
            foreach(KeyValuePair<string, Dictionary<string, PropertyInfo>> Table in this.TableStructs)
            {
                Dictionary<PropertyInfo, PropertyInfo> ForeignProps = new Dictionary<PropertyInfo, PropertyInfo>();
                foreach (KeyValuePair<string, PropertyInfo> Column in Table.Value)
                {
                    DbPropAttribute dpa = null;
                    if (!Wrapper.IsQueryAble(Column.Value, ref dpa) || string.IsNullOrEmpty(dpa.DbName))
                        throw new DbAttributeException();

                    if (dpa is IForeignKeyPropAttribute)
                        ForeignProps.Add(Column.Value, this.TableStructs[((IForeignKeyPropAttribute)dpa).ReferenceTable][((IForeignKeyPropAttribute)dpa).ReferenceDbName]);
                }
                this.ClassByTable[Table.Key].GetCustomAttribute<DbClassAttribute>().ForeignKeys = new ForeignKeyStruct(ForeignProps);
            }
        }

        #region IsQueryAble

        private static bool IsQueryAble(Type t)
        {
            return t.GetCustomAttribute<DbClassAttribute>() != null;
        }

        private static bool IsQueryAble(Type t, ref DbClassAttribute typeAttribute)
        {
            return (typeAttribute = t.GetCustomAttribute<DbClassAttribute>()) != null;
        }

        private static bool IsQueryAble(PropertyInfo prop)
        {
            DbPropAttribute dpa = null;
            return Wrapper.IsQueryAble(prop, ref dpa);
        }

        private static bool IsQueryAble(PropertyInfo prop, ref DbPropAttribute propAttribute)
        {
            int nbAttributes = 0;
            DbPropAttribute dpa = null;
            if ((dpa = prop.GetCustomAttribute<DbPropAttribute>()) != null)
                nbAttributes++;
            if ((dpa = prop.GetCustomAttribute<DbPrimaryKeyPropAttribute>()) != null)
                nbAttributes++;
            if ((dpa = prop.GetCustomAttribute<DbForeignKeyPropAttribute>()) != null)
                nbAttributes++;
            if ((dpa = prop.GetCustomAttribute<DbPrimaryForeignKeyPropAttribute>()) != null)
                nbAttributes++;

            if (nbAttributes > 1)
                throw new DbPropAttributeException($"La propriété {prop.ReflectedType.FullName}.{prop.Name} doit avoir un seul attribut de type {typeof(DbPropAttribute).FullName}.");

            return dpa != null && nbAttributes == 1;
        }

        #endregion

        public BaseQuery<Query> CreateSelectAllRequest<T>(QueryFactory qf) where T : BaseEntity
        {
            DbClassAttribute dca = null;
            if (!Wrapper.IsQueryAble(typeof(T), ref dca))
                throw new DbClassAttributeException();

            Query q = qf.Query(dca.DbName)
                .Select(this.GetFullNameProps<T>().ToArray());

            throw new NotImplementedException();
        }

        public BaseQuery<Query> CreateSelectByPrimaryKeyRequest<T>(QueryFactory qf, PrimaryKeyStruct id) where T : BaseEntity
        {
            throw new NotImplementedException();
        }

        public BaseQuery<Query> CreateUpdateRequest<T>(QueryFactory qf, T Entity) where T : BaseEntity
        {
            throw new NotImplementedException();
        }

        public BaseQuery<Query> CreateDeleteRequest<T>(QueryFactory qf, T Entity) where T : BaseEntity
        {
            throw new NotImplementedException();
        }

        #region Query Building

        /// <summary>
        /// Obtient l'arbre d'héritage en base de données du Type donné.
        /// </summary>
        /// <param name="t">Type à analyser.</param>
        /// <returns>Liste contenant les noms en base de données des parents du Type.</returns>
        private List<string> GetClassExtendsTree(Type t)
        {
            List<string> res = new List<string>();

            DbClassAttribute dca = null;
            if(!Wrapper.IsQueryAble(t, ref dca) || !t.IsSubclassOf(typeof(BaseEntity)))
                throw new DbClassAttributeException();

            if (string.IsNullOrEmpty(dca.DbExtends))
                return new List<string>();

            res.Add(dca.DbExtends);
            try { return (List<string>)res.Concat(this.GetClassExtendsTree(this.ClassByTable[dca.DbExtends])); }
            catch(ArgumentException e) { throw new DbClassAttributeException($"L'assembly ne contient aucune Type avec un attribut {dca.GetType().FullName} ayant DbName à \"{dca.DbExtends}\"", e); }
        }

        /// <summary>
        /// Donne le nom complet de toutes les propriétés en base de données.
        /// </summary>
        /// <typeparam name="T">Type à analyser.</typeparam>
        /// <returns>Liste des noms complet au format base de données suivant : TABLE.PROPRIETE.</returns>
        private IEnumerable<string> GetFullNameProps<T>() where T : BaseEntity
        {
            foreach(string Table in this.GetClassExtendsTree(typeof(T)))
            {
                foreach (KeyValuePair<string, PropertyInfo> kvp in this.TableStructs[Table])
                {
                    yield return $"{Table}.{kvp.Key}";
                }
            }
        }

        /// <summary>
        /// Ajoute les jointures à une requête, correspondant à l'arbre d'héritage du Type donné.
        /// </summary>
        /// <typeparam name="T">Type utilisé dans la requête.</typeparam>
        /// <param name="query">Objet Query où ajouter les jointures.</param>
        /// <returns>Objet Query avec les jointures ajoutées.</returns>
        private Query MakeInheritanceJoins<T>(Query query) where T : BaseEntity
        {
            DbClassAttribute child_dca = null;
            if (!Wrapper.IsQueryAble(typeof(T), ref child_dca))
                throw new DbClassAttributeException();

            // On parcourt toutes les tables parents
            foreach (string Table in this.GetClassExtendsTree(typeof(T)))
            {
                DbClassAttribute parent_dca = null;
                if (!Wrapper.IsQueryAble(this.ClassByTable[Table], ref parent_dca))
                    throw new DbClassAttributeException();

                // On ajoute la jointure pour chaque parent
                query = query.Join(Table, j =>
                {
                    int ReferenceKeyCount = 0;
                    // On parcourt les différentes clés primaires de la table enfant pour trouver celles associées à la table parent
                    foreach(KeyValuePair<PropertyInfo, PropertyInfo> ChildForeignKey in child_dca.ForeignKeys)
                    {
                        IForeignKeyPropAttribute dpa = null;
                        if ((dpa = ChildForeignKey.Key.GetCustomAttribute<DbForeignKeyPropAttribute>()) == null)
                            if ((dpa = ChildForeignKey.Key.GetCustomAttribute<DbPrimaryForeignKeyPropAttribute>()) == null)
                                throw new DbPropAttributeException();

                        // On fait la jointure si la clé primaire du parent est trouvée
                        if (dpa.IsInheritanceKey && dpa.ReferenceTable == Table)
                        {
                            j = j.On($"{child_dca.DbName}.{((DbPropAttribute)dpa).DbName}", $"{dpa.ReferenceTable}.{dpa.ReferenceDbName}");
                            ReferenceKeyCount++;
                        }
                    }

                    if (ReferenceKeyCount < 1)
                        throw new DbClassAttributeException($"Le Type {this.ClassByTable[child_dca.DbName].FullName} est marqué comme étant un enfant de la table {Table} mais ne contient aucune clé étrangère associée.");

                    return j;
                });

                child_dca = parent_dca;
            }

            return query;
        }

        #endregion
    }
}
