using KAutoFactWrapper;
using KAutoFactWrapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsKAutoFactWrapper.TestClasses
{
    [DbClass("BAR", "FOO")]
    public abstract class Bar<TChildReference> : Foo<TChildReference> where TChildReference : Bar<TChildReference>
    {
        [DbProp("BAR_ITEM")]
        public int BarItem { get; set; }

        public Bar(int id) : base(id) { }
    }
}
