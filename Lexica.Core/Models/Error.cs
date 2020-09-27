using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Models
{
    class Error
    {
        public string Code { get; private set; }

        public string Message { get; private set; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    class Error<T> : Error
    {
        public T Data { get; private set; }

        public Error(string code, string message, T data) : base(code, message)
        {
            Data = data;
        }
    }
}
