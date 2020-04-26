using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DbClassAttribute : DbAttribute
    {
        public string DbExtends { get; private set; }

        public DbClassAttribute(string name) : this(name, "") { }
        public DbClassAttribute(string name, string dbExtends) : base(name)
        {
            this.DbExtends = dbExtends;
        }
    }
}
