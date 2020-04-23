using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper
{
    public class ForeignKey
    {
        public PropertyInfo Item { get; private set; }
        public PropertyInfo Reference { get; private set; }

        public ForeignKey(PropertyInfo item, PropertyInfo reference)
        {
            this.Item = item;
            this.Reference = reference;
        }
    }
}
