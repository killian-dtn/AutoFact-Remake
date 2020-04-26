using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KAutoFactWrapper;
using KAutoFactWrapper.Attributes;

namespace TestsKAutoFactWrapper.TestClasses
{
    public class Foo<TChildReference> : BaseEntity<TChildReference> where TChildReference : Foo<TChildReference>
    {
        public int Id { get; private set; }
        public int FooItem { get; private set; }
    }
}
