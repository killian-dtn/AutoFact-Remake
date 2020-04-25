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
    public class PrimaryKeyStruct : IEnumerable
    {
        public PropertyInfo[] PrimaryKeyProps { get; private set; }
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

        private static Wrapper w = Wrapper.Instance;

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
            return new PrimaryKeyEnumerator(this.PrimaryKeyProps, this.PrimaryKeyFullNames);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PrimaryKeyStruct))
                return false;
            if (((PrimaryKeyStruct)obj).PrimaryKeyProps.Length != this.PrimaryKeyProps.Length)
                return false;
            if (!((PrimaryKeyStruct)obj).AssociatedType.Equals(this.AssociatedType))
                return false;

            foreach (PropertyInfo p in (PrimaryKeyStruct)obj)
                if (!this.PrimaryKeyProps.Contains<PropertyInfo>(p))
                    return false;

            return true;
        }
    }
}
