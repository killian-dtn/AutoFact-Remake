using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper.Exceptions
{
    internal class DbClassAttributeException : DbAttributeException
    {
        public DbClassAttributeException() : base() { }
        public DbClassAttributeException(string message) : base(message) { }
        public DbClassAttributeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
