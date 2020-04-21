using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoFactWrapper.Exceptions
{
    internal class DbAttributeException : Exception
    {
        public DbAttributeException() : base() { }
        public DbAttributeException(string message) : base(message) { }
        public DbAttributeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
