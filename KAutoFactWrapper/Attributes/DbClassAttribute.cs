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
        public bool AutoIncrement { get; private set; }

        public DbClassAttribute(string name) : this(name, "", false) { }
        public DbClassAttribute(string name, string dbExtends) : this(name, dbExtends, false) { }
        public DbClassAttribute(string name, string dbExtends, bool autoIncrement) : base(name)
        {
            this.DbExtends = dbExtends;
            this.AutoIncrement = autoIncrement;
        }
    }
}
