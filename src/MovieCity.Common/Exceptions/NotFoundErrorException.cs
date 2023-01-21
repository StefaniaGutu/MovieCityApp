using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.Common.Exceptions
{
    public class NotFoundErrorException : Exception
    {
        public NotFoundErrorException(string message) : base(message)
        {
        }

        public NotFoundErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NotFoundErrorException()
        {
        }
    }
}
