﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DbForeignKeyPropAttribute : DbPropAttribute, IForeignKeyPropAttribute
    {
        public string ReferenceDbName { get; private set; }
        public string ReferenceTable { get; private set; }
        public RelationType Relation { get; private set; }
        public bool IsInheritanceKey { get; private set; }
        string IForeignKeyPropAttribute.ReferenceDbName { get { return this.ReferenceDbName; } set { this.ReferenceDbName = value; } }
        string IForeignKeyPropAttribute.ReferenceTable { get { return this.ReferenceTable; } set { this.ReferenceTable = value; } }
        RelationType IForeignKeyPropAttribute.Relation { get { return this.Relation; } set { this.Relation = value; } }
        bool IForeignKeyPropAttribute.IsInheritanceKey { get { return this.IsInheritanceKey; } set { this.IsInheritanceKey = value; } }

        private DbForeignKeyPropAttribute(string dbName, string referenceDbName, string referenceTable, RelationType relation, bool isInheritanceKey = false) : base(dbName)
        {
            this.ReferenceDbName = referenceDbName;
            this.ReferenceTable = referenceTable;
            this.Relation = relation;
            this.IsInheritanceKey = isInheritanceKey;
        }

        public DbForeignKeyPropAttribute(string dbName, string referenceDbName, string referenceTable, bool isInheritanceKey = false) : this(dbName, referenceDbName, referenceTable, RelationType.none, isInheritanceKey) { }
    }
}
