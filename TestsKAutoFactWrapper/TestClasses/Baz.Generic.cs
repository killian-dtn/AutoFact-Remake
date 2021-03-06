﻿using KAutoFactWrapper;
using KAutoFactWrapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsKAutoFactWrapper.TestClasses
{
    [DbClass("BAZ", "BAR")]
    public abstract class Baz<TChildReference> : Bar<TChildReference> where TChildReference : Baz<TChildReference>
    {
        [DbProp("BAZ_ITEM")]
        public int BazItem { get; set; }

        public Baz(int id) : base(id) { }
    }
}
