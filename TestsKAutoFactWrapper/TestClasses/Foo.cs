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
    class Foo : BaseEntity
    {
        [DbPrimaryKeyProp("ID")]
        public int Id { get; private set; }
        [DbProp("FOO_ITEM")]
        public int FooItem { get; private set; }
    }
}
