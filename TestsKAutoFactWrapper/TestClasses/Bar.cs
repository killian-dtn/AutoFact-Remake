using KAutoFactWrapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsKAutoFactWrapper.TestClasses
{
    [DbClass("BAR", "FOO")]
    public sealed class Bar : Bar<Bar>
    {
        [DbPrimaryForeignKeyProp("ID", "ID", "FOO")]
        public override int Id { get; protected set; }

        public Bar(int id) : base(id) { }
    }
}
