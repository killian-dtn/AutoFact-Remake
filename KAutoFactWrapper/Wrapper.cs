﻿using System;
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

        public Dictionary<Type, string> TableByClass { get; private set; }
        public Dictionary<string, Type> ClassByTable { get; private set; }
        public Dictionary<string, Dictionary<string, PropertyInfo>> TableStructs { get; private set; }

        private static Wrapper instance = null;
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
                        List<PropertyInfo> PrimaryProps = null;
                        Dictionary<string, PropertyInfo> TableStructTmp = new Dictionary<string, PropertyInfo>();
                        foreach(PropertyInfo prop in t.GetProperties())
                        {
                            DbPropAttribute dpa = null;
                            if (!Wrapper.IsQueryAble(prop, ref dpa))
                                continue;

                            if (string.IsNullOrEmpty(dpa.Name))
                                continue;

                            if (HasPrimaryKey |= dpa.IsPrimaryKey)
                                PrimaryProps.Add(prop);

                            try { TableStructTmp.Add(dpa.Name, prop); }
                            catch(ArgumentException e) { throw new DbPropAttributeException($"La valeur Name de l'attribut {typeof(DbPropAttribute).FullName} \"{dpa.Name}\" est utilisée plusieurs fois dans la classe {t.FullName}.", e); }
                        }

                        if (!HasPrimaryKey)
                            throw new DbPropAttributeException($"Le type {t.FullName} n'a pas de clé primaire.");

                        dca.PrimaryKey = new PrimaryKeyStruct(t, PrimaryProps.ToArray());

                        this.TableByClass.Add(t, dca.Name);

                        try
                        {
                            this.ClassByTable.Add(dca.Name, t);
                            this.TableStructs.Add(dca.Name, TableStructTmp);
                        }
                        catch (ArgumentNullException e) { throw new DbClassAttributeException($"La valeur Name de l'attribut {typeof(DbClassAttribute).FullName} sur le type {t.FullName} est nulle.", e); }
                        catch (ArgumentException e) { throw new DbClassAttributeException($"La valeur Name de l'attribut {typeof(DbClassAttribute).FullName} \"{dca.Name}\" est utilisée sur plusieurs classes.", e); }
                    }
                }
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
            return prop.GetCustomAttribute<DbPropAttribute>() != null;
        }

        private static bool IsQueryAble(PropertyInfo prop, ref DbPropAttribute propAttribute)
        {
            return (propAttribute = prop.GetCustomAttribute<DbPropAttribute>()) != null;
        }

        #endregion

        public BaseQuery<Query> CreateSelectAllRequest<T>(QueryFactory qf) where T : BaseEntity
        {
            DbClassAttribute dca = null;
            if (!Wrapper.IsQueryAble(typeof(T), ref dca))
                throw new DbClassAttributeException();

            Query q = qf.Query(dca.Name)
                .Select(this.GetFullNameProps<T>().ToArray());

            //q = this.MakeJoins<T>(q);

            //return q;

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
            catch(ArgumentException e) { throw new DbClassAttributeException($"L'assembly ne contient aucune Type avec un attribut {dca.GetType().FullName} ayant Name à \"{dca.DbExtends}\"", e); }
        }

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

        #endregion
    }
}
