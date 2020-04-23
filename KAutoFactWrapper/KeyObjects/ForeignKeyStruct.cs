using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper
{
    class ForeignKeyStruct : IEnumerable
    {
        public ForeignKey[] ForeignKeys { get; private set; }

        public ForeignKeyStruct(ForeignKey[] foreignKeys)
        {
            this.ForeignKeys = foreignKeys;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return new ForeignKeyEnumerator(this.ForeignKeys);
        }
    }
}
