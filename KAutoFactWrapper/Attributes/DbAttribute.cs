using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper.Attributes
{
    public abstract class DbAttribute : Attribute
    {
        public string Name { get; private set; }

        public DbAttribute(string name)
        {
            this.Name = name;
        }
    }
}
