using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Exceptions
{
    public class WrongConfigException : AppException
    {
        public WrongConfigException() { }

        public WrongConfigException(string message) : base(message) { }

        public WrongConfigException(string message, Exception inner) : base(message, inner) { }
    }
}
