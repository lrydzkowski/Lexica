﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Models
{
    class OperationResult
    {
        public bool Result { get; private set; }

        public List<Error> Errors { get; private set; }

        public OperationResult(bool result, List<Error> errors = null)
        {
            Result = result;
            Errors = errors;
        }
    }

    class OperationResult<T> : OperationResult
    {
        public T Data { get; private set; }

        public OperationResult(bool result, T data, List<Error> errors = null) : base(result, errors)
        {
            Data = data;
        }
    }
}
