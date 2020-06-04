using KAutoFactWrapper.Attributes;
using KAutoFactWrapper.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper
{
    public abstract class BaseEntity<TChildReference> where TChildReference : BaseEntity<TChildReference>
    {
        protected Wrapper Wrapper_;
        protected DbConnection Connection;

        public BaseEntity()
        {
            this.Wrapper_ = Wrapper.Instance;
            this.Connection = DbConnection.Instance;
        }

        public string GetDbTableName() { return this.Wrapper_.TableByClass[((TChildReference)this).GetType()]; }

        public object GetDbPropValue(string PropDbName)
        {
            PropertyInfo Prop = null;

            try
            {
                Prop = this.Wrapper_.TableStructs[((TChildReference)this).GetDbTableName()][PropDbName];
                return Prop.GetValue(((TChildReference)this));
            }
            catch (ArgumentOutOfRangeException e) { throw new DbPropAttributeException(
                $"La classe {this.GetType().FullName} n'a pas de propriété avec {typeof(DbPropAttribute).FullName}.DbName ayant la valeur \"{PropDbName}\".", e
            ); }
            catch (ArgumentException) { throw; }
        }

        #region Request

        public static object SelectAll()
        {
            return DbConnection.Instance.SelectAll<TChildReference>();
        }

        public static TChildReference SelectByPrimaryKey(Dictionary<string, object> PrimaryKeys)
        {
            return DbConnection.Instance.SelectByPrimaryKey<TChildReference>(PrimaryKeys);
        }

        public void Insert()
        {
            this.Connection.Insert<TChildReference>((TChildReference)this);
        }

        public void Update()
        {
            this.Connection.Update<TChildReference>((TChildReference)this);
        }

        public void Delete()
        {
            this.Connection.Delete<TChildReference>((TChildReference)this);
        }

        #endregion
    }
}
