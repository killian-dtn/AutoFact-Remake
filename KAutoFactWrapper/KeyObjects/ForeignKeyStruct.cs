using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KAutoFactWrapper.Exceptions;
using Renci.SshNet.Security;

namespace KAutoFactWrapper
{
    public class ForeignKeyStruct : ICollection<KeyValuePair<PropertyInfo, PropertyInfo>>
    {
        public Dictionary<PropertyInfo, PropertyInfo> ForeignKeys { get; private set; }
        public Type AssociatedType { get; private set; }
        public PropertyInfo this[PropertyInfo Item]
        {
            get
            {
                try { return this.ForeignKeys[Item]; }
                catch (IndexOutOfRangeException e) { throw new DbAttributeException($"{Item.ReflectedType.FullName}.{Item.Name} n'est pas répertoriée comme clé étrangère.", e); }
            }
        }
        public KeyValuePair<PropertyInfo, PropertyInfo> this[int index]
        {
            get
            {
                try { return this.ForeignKeys.ElementAt(index); }
                catch (ArgumentNullException) { throw; }
                catch (ArgumentOutOfRangeException) { throw; }
            }
        }

        public ForeignKeyStruct(Type associatedType) : this(associatedType, new Dictionary<PropertyInfo, PropertyInfo>()) { }
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

        IEnumerator<KeyValuePair<PropertyInfo, PropertyInfo>> IEnumerable<KeyValuePair<PropertyInfo, PropertyInfo>>.GetEnumerator()
        {
            return (IEnumerator<KeyValuePair<PropertyInfo, PropertyInfo>>)this.GetEnumerator();
        }

        public ForeignKeyEnumerator GetEnumerator()
        {
            return new ForeignKeyEnumerator(this);
        }

        #endregion

        #region ICollection

        public int Count { get { return this.ForeignKeys.Count; } }
        public bool IsSynchronized { get { return false; } }
        public object SyncRoot { get { return this; } }
        public bool IsReadOnly { get { return false; } }

        bool ICollection<KeyValuePair<PropertyInfo, PropertyInfo>>.Contains(KeyValuePair<PropertyInfo, PropertyInfo> item) { return this.Contains(item.Key, item.Value); }
        public bool Contains(PropertyInfo Item, PropertyInfo Reference) { return this.ForeignKeys.Contains(new KeyValuePair<PropertyInfo, PropertyInfo>(Item, Reference)); }

        void ICollection<KeyValuePair<PropertyInfo, PropertyInfo>>.Add(KeyValuePair<PropertyInfo, PropertyInfo> item) { this.Add(item.Key, item.Value); }
        public void Add(PropertyInfo Item, PropertyInfo Reference) { this.ForeignKeys.Add(Item, Reference); }

        public void Clear() { this.ForeignKeys.Clear(); }

        void ICollection<KeyValuePair<PropertyInfo, PropertyInfo>>.CopyTo(KeyValuePair<PropertyInfo, PropertyInfo>[] array, int arrayIndex)
        {
            this.ForeignKeys.ToList<KeyValuePair<PropertyInfo, PropertyInfo>>().CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<PropertyInfo, PropertyInfo>>.Remove(KeyValuePair<PropertyInfo, PropertyInfo> item) { return this.ForeignKeys.ToList<KeyValuePair<PropertyInfo, PropertyInfo>>().Remove(item); }
        public bool Remove(PropertyInfo Item, PropertyInfo Reference) { return ((ICollection<KeyValuePair<PropertyInfo, PropertyInfo>>)this).Remove(new KeyValuePair<PropertyInfo, PropertyInfo>(Item, Reference)); }

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

        public override int GetHashCode()
        {
            int hashCode = -1880409550;
            hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<PropertyInfo, PropertyInfo>>.Default.GetHashCode(ForeignKeys);
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(AssociatedType);
            hashCode = hashCode * -1521134295 + Count.GetHashCode();
            hashCode = hashCode * -1521134295 + IsSynchronized.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(SyncRoot);
            hashCode = hashCode * -1521134295 + IsReadOnly.GetHashCode();
            return hashCode;
        }
    }
}
