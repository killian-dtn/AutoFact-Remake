﻿using KAutoFactWrapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsKAutoFactWrapper.TestClasses
{
    [DbClass("BAZ", "BAR")]
    public sealed class Baz : Baz<Baz>
    {
        [DbPrimaryForeignKeyProp("ID", "ID", "BAR")]
        public new int Id { get; private set; }

        public Baz(int id) : base(id) { }
    }
}
