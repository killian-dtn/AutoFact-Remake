using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DbPropAttribute : DbAttribute
    {
        public bool IsPrimaryKey { get; private set; }
        public bool IsForeignKey { get { return this.Relation == RelationType.one_n || this.Relation == RelationType.one_one; } }
        public bool IsMandatory { get; private set; }
        public RelationType Relation { get; private set; }

        public DbPropAttribute(string name, bool isPrimaryKey = false, RelationType relation = RelationType.none, bool isMandatory = false) : base(name)
        {
            this.IsPrimaryKey = isPrimaryKey;
            this.Relation = relation;
            this.IsMandatory = isMandatory;
        }
    }
}
