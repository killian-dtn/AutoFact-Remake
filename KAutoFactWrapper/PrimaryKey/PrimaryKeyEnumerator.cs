using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper
{
    public class PrimaryKeyEnumerator : IEnumerator
    {
        public PropertyInfo[] PrimaryKeyProps { get; private set; }
        public PropertyInfo Current
        {
            get
            {
                try { return this.PrimaryKeyProps[this.Position]; }
                catch(IndexOutOfRangeException) { throw new InvalidOperationException(); }
            }
        }

        private int Position;

        public PrimaryKeyEnumerator(PropertyInfo[] primaryKeyProps)
        {
            this.PrimaryKeyProps = primaryKeyProps;
            this.Position = -1;
        }

        public bool MoveNext()
        {
            this.Position++;
            return (this.Position < this.PrimaryKeyProps.Length);
        }

        public void Reset()
        {
            this.Position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }
    }
}
