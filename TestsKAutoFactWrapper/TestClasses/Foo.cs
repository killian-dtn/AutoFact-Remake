﻿using KAutoFactWrapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsKAutoFactWrapper.TestClasses
{
    [DbClass("FOO")]
    public sealed class Foo : Foo<Foo>
    {
        [DbPrimaryKeyProp("ID")]
        public new int Id { get; private set; }
        [DbProp("FOO_ITEM")]
        public new int FooItem { get; private set; }
    }
}
