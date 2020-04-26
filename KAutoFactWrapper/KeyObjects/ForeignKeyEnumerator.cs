using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper
{
    public class ForeignKeyEnumerator : IEnumerator<KeyValuePair<PropertyInfo, PropertyInfo>>
    {
        private ForeignKeyStruct FKStruct;
        public KeyValuePair<PropertyInfo, PropertyInfo> Current
        {
            get
            {
                try { return this.FKStruct.ForeignKeys.ElementAt(this.Position); }
                catch(IndexOutOfRangeException) { throw new InvalidOperationException(); }
            }
        }
        object IEnumerator.Current { get { return this.Current; } }
        public int Position { get; private set; }

        public ForeignKeyEnumerator(ForeignKeyStruct Struct)
        {
            this.FKStruct = Struct;
            this.Position = -1;
        }

        public bool MoveNext()
        {
            this.Position++;
            return (this.Position < this.FKStruct.Count);
        }

        public void Reset()
        {
            this.Position = -1;
        }

        void IDisposable.Dispose() { }
    }
}
