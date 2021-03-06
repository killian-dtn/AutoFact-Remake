﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper.Attributes
{
    internal interface IForeignKeyPropAttribute
    {
        string ReferenceDbName { get; set; }
        string ReferenceTable { get; set; }
        RelationType Relation { get; set; }
        bool IsInheritanceKey { get; set; }
    }
}
