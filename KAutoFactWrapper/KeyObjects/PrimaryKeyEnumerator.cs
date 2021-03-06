﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper
{
    public class PrimaryKeyEnumerator : IEnumerator<PropertyInfo>
    {
        public PrimaryKeyStruct PrimaryKeyProps { get; private set; }
        public PropertyInfo Current
        {
            get
            {
                try { return this.PrimaryKeyProps[this.Position]; }
                catch(IndexOutOfRangeException) { throw new InvalidOperationException(); }
            }
        }
        object IEnumerator.Current { get { return this.Current; } }
        public string CurrentFullName
        {
            get
            {
                try { return this.PrimaryKeyProps.PrimaryKeyFullNames[this.Position]; }
                catch (IndexOutOfRangeException) { throw new InvalidOperationException(); }
            }
        }
        public int Position { get; private set; }

        public PrimaryKeyEnumerator(PrimaryKeyStruct primaryKeyProps)
        {
            this.PrimaryKeyProps = primaryKeyProps;
            this.Position = -1;
        }

        public bool MoveNext()
        {
            this.Position++;
            return (this.Position < this.PrimaryKeyProps.Count);
        }

        public void Reset()
        {
            this.Position = -1;
        }

        void IDisposable.Dispose() { }
    }
}
