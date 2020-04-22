using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper
{
    public class PrimaryKeyStruct : IEnumerable
    {
        public PropertyInfo[] PrimaryKeyProps { get; private set; }
        public Type AssociatedType { get; private set; }

        public PrimaryKeyStruct(Type associatedType, params PropertyInfo[] primaryKeyProps)
        {
            this.AssociatedType = associatedType;
            this.PrimaryKeyProps = primaryKeyProps;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }

        public PrimaryKeyEnumerator GetEnumerator()
        {
            return new PrimaryKeyEnumerator(this.PrimaryKeyProps);
        }
    }
}
