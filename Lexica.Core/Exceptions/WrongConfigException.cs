using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Exceptions
{
    [Serializable]
    public class WrongConfigException : ApplicationException
    {
        public WrongConfigException() { }

        public WrongConfigException(string message) : base(message) { }

        public WrongConfigException(string message, Exception inner) : base(message, inner) { }

        protected WrongConfigException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
