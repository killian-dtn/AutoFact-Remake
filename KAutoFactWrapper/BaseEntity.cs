using System;
using System.Collections.Generic;
using System.Linq;
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
            this.Connection.Insert<TChildReference>(this);
        }

        public void Update()
        {
            this.Connection.Update<TChildReference>(this);
        }

        public void Delete()
        {
            this.Connection.Delete<TChildReference>(this);
        }
    }
}
