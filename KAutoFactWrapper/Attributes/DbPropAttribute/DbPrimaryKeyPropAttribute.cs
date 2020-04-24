using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DbPrimaryKeyPropAttribute : DbPropAttribute, IPrimaryKeyPropAttribute
    {
        public DbPrimaryKeyPropAttribute(string dbName) : base(dbName) { }
    }
}
