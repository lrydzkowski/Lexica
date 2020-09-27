using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Exceptions
{
    public class AppException : Exception
    {
        public AppException() { }

        public AppException(string message) : base(message) { }

        public AppException(string message, Exception inner) : base(message, inner) { }
    }
}
