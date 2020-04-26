using KAutoFactWrapper;
using KAutoFactWrapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsKAutoFactWrapper.TestClasses
{
    public class Baz<TChildReference> : Bar<TChildReference> where TChildReference : Baz<TChildReference>
    {
        public new int Id { get; private set; }
        public int BazItem { get; private set; }
    }
}
