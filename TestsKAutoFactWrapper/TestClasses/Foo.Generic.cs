using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KAutoFactWrapper;
using KAutoFactWrapper.Attributes;

namespace TestsKAutoFactWrapper.TestClasses
{
    [DbClass("FOO")]
    public abstract class Foo<TChildReference> : BaseEntity<TChildReference> where TChildReference : Foo<TChildReference>
    {
        public virtual int Id { get; protected set; }
        [DbProp("FOO_ITEM")]
        public int FooItem { get; set; }

        public Foo(int id) { this.Id = id; }
    }
}
