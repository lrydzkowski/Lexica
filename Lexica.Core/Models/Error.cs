using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Models
{
    public class Error
    {
        public int Code { get; private set; }

        public string Message { get; private set; }

        public Error(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    public class Error<T> : Error
    {
        public T Data { get; private set; }

        public Error(int code, string message, T data) : base(code, message)
        {
            Data = data;
        }
    }
}
