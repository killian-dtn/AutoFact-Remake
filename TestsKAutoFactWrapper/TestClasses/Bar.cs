﻿using KAutoFactWrapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsKAutoFactWrapper.TestClasses
{
    [DbClass("BAR", "FOO")]
    class Bar : Foo
    {
        [DbPrimaryForeignKeyProp("ID", "ID", "FOO")]
        public new int Id { get; private set; }
        [DbProp("BAR_ITEM")]
        public int BarItem { get; private set; }
    }
}
