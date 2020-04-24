using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DbPropAttribute : DbAttribute
    {
        public DbPropAttribute(string dbName) : base(dbName) { }
    }
}
