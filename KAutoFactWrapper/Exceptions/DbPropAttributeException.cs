using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper.Exceptions
{
    internal class DbPropAttributeException : DbAttributeException
    {
        public DbPropAttributeException() : base() { }
        public DbPropAttributeException(string message) : base(message) { }
        public DbPropAttributeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
