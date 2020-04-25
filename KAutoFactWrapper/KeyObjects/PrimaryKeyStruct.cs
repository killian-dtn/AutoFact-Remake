using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KAutoFactWrapper.Attributes;
using KAutoFactWrapper.Exceptions;

namespace KAutoFactWrapper
{
    public class PrimaryKeyStruct : ICollection<PropertyInfo>
    {
        public List<PropertyInfo> PrimaryKeyProps { get; private set; }
        public Type AssociatedType { get; private set; }
        public string[] PrimaryKeyFullNames
        {
            get
            {
                List<string> res = new List<string>();
                foreach (PropertyInfo prop in this.PrimaryKeyProps)
                {
                    try { res.Add($"{PrimaryKeyStruct.w.TableByClass[prop.ReflectedType]}.{prop.GetCustomAttribute<DbPropAttribute>().DbName}"); }
                    catch (ArgumentException e) { throw new DbClassAttributeException(e.Message, e); }
                    catch (NullReferenceException e) { throw new DbPropAttributeException($"La propriété {prop.ReflectedType.FullName}.{prop.Name} ne respecte pas les paramètres nécessaire au Wrapper.", e); }
                }

                return res.ToArray();
            }
        }
        public int Count { get { return this.PrimaryKeyProps.Count; } }
        public bool IsReadOnly { get { return false; } }
        public PropertyInfo this[int index] { get { return this.PrimaryKeyProps[index]; } }

        private static Wrapper w = Wrapper.Instance;

        public PrimaryKeyStruct(Type associatedType) : this(associatedType, new PropertyInfo[] { }) { }
        public PrimaryKeyStruct(Type associatedType, params PropertyInfo[] primaryKeyProps)
        {
            this.AssociatedType = associatedType;
            this.PrimaryKeyProps = primaryKeyProps.ToList();
        }

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() { return (IEnumerator)this.GetEnumerator(); }
        IEnumerator<PropertyInfo> IEnumerable<PropertyInfo>.GetEnumerator() { return (IEnumerator<PropertyInfo>)this.GetEnumerator(); }
        public PrimaryKeyEnumerator GetEnumerator() { return new PrimaryKeyEnumerator(this); }

        #endregion

        #region ICollection

        public bool Contains(PropertyInfo item) { return this.PrimaryKeyProps.Contains<PropertyInfo>(item); }
        public void Add(PropertyInfo item) { this.PrimaryKeyProps.Add(item); }
        public void Clear() { this.PrimaryKeyProps.Clear(); }
        public void CopyTo(PropertyInfo[] array, int arrayIndex) { this.PrimaryKeyProps.CopyTo(array, arrayIndex); }
        public bool Remove(PropertyInfo item) { return this.PrimaryKeyProps.Remove(item); }

        #endregion

        public override bool Equals(object obj)
        {
            if (!(obj is PrimaryKeyStruct))
                return false;
            if (((PrimaryKeyStruct)obj).PrimaryKeyProps.Count != this.PrimaryKeyProps.Count)
                return false;
            if (!((PrimaryKeyStruct)obj).AssociatedType.Equals(this.AssociatedType))
                return false;

            foreach (PropertyInfo p in (PrimaryKeyStruct)obj)
                if (!this.PrimaryKeyProps.Contains<PropertyInfo>(p))
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = 1095920675;
            hashCode = hashCode * -1521134295 + EqualityComparer<List<PropertyInfo>>.Default.GetHashCode(PrimaryKeyProps);
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(AssociatedType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(PrimaryKeyFullNames);
            hashCode = hashCode * -1521134295 + Count.GetHashCode();
            hashCode = hashCode * -1521134295 + IsReadOnly.GetHashCode();
            return hashCode;
        }
    }
}
