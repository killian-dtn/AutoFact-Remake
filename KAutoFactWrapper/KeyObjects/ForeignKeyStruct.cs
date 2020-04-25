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
    public class ForeignKeyStruct : IEnumerable
    {
        public Dictionary<PropertyInfo, PropertyInfo> ForeignKeys { get; private set; }
        public Type AssociatedType { get; private set; }
        public PropertyInfo this[PropertyInfo Item]
        {
            get
            {
                try { return this.ForeignKeys[Item]; }
                catch(IndexOutOfRangeException e) { throw new DbAttributeException($"{Item.ReflectedType.FullName}.{Item.Name} n'est pas répertoriée comme clé étrangère.", e); }
            }
        }

        public ForeignKeyStruct(Type associatedType, Dictionary<PropertyInfo, PropertyInfo> foreignKeys)
        {
            this.AssociatedType = associatedType;
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

        public override bool Equals(object obj)
        {
            if (!(obj is ForeignKeyStruct))
                return false;
            if (((ForeignKeyStruct)obj).ForeignKeys.Count != this.ForeignKeys.Count)
                return false;
            if (!((ForeignKeyStruct)obj).AssociatedType.Equals(this.AssociatedType))
                return false;

            try
            {
                foreach (KeyValuePair<PropertyInfo, PropertyInfo> fk in (ForeignKeyStruct)obj)
                    if (!this.ForeignKeys[fk.Key].Equals(fk.Value))
                        return false;
            }
            catch (IndexOutOfRangeException) { return false; }

            return true;
        }
    }
}
