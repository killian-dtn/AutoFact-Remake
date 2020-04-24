using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DbClassAttribute : DbAttribute
    {
        public string DbExtends { get; private set; }
        public PrimaryKeyStruct PrimaryKey { get; internal set; }
        public ForeignKeyStruct ForeignKeys { get; internal set; }

        public DbClassAttribute(string name, string dbExtends) : base(name)
        {
            this.DbExtends = dbExtends;
            this.PrimaryKey = null;
            this.ForeignKeys = null;
        }
    }
}
