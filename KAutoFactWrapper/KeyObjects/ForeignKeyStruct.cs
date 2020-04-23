using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KAutoFactWrapper.Exceptions;

namespace KAutoFactWrapper
{
    class ForeignKeyStruct : IEnumerable
    {
        public Dictionary<PropertyInfo, PropertyInfo> ForeignKeys { get; private set; }
        public PropertyInfo this[PropertyInfo Item]
        {
            get
            {
                try { return this.ForeignKeys[Item]; }
                catch(IndexOutOfRangeException e) { throw new DbAttributeException($"{Item.ReflectedType.FullName}.{Item.Name} n'est pas répertoriée comme clé étrangère.", e); }
            }
        }

        public ForeignKeyStruct(Dictionary<PropertyInfo, PropertyInfo> foreignKeys)
        {
            this.ForeignKeys = foreignKeys;
        }

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return new ForeignKeyEnumerator(this.ForeignKeys);
        }

        #endregion
    }
}
