using KAutoFactWrapper;
using KAutoFactWrapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsKAutoFactWrapper.TestClasses
{
    [DbClass("BAZ", "BAR")]
    class Baz : BaseEntity<Baz>
    {
        [DbPrimaryForeignKeyProp("ID", "ID", "BAR")]
        public int Id { get; private set; }
        [DbProp("BAZ_ITEM")]
        public int BazItem { get; private set; }
    }
}
