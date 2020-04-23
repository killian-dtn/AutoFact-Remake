using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper
{
    class ForeignKeyEnumerator : IEnumerator
    {
        public ForeignKey[] ForeignKeys { get; private set; }
        public ForeignKey Current
        {
            get
            {
                try { return this.ForeignKeys[this.Position]; }
                catch(IndexOutOfRangeException) { throw new InvalidOperationException(); }
            }
        }
        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }
        public int Position { get; private set; }

        public ForeignKeyEnumerator(ForeignKey[] foreignKeys)
        {
            this.ForeignKeys = foreignKeys;
            this.Position = -1;
        }

        public bool MoveNext()
        {
            this.Position++;
            return (this.Position < this.ForeignKeys.Length);
        }

        public void Reset()
        {
            this.Position = -1;
        }
    }
}
