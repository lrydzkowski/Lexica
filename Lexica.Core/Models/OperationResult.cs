﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Models
{
    public class OperationResult
    {
        public bool Result { get; private set; }

        public List<Error> Errors { get; private set; }

        public OperationResult()
        {
            Result = true;
        }

        public OperationResult(bool result)
        {
            Result = result;
        }

        public OperationResult(bool result, Error error)
        {
            Result = result;
            if (error != null)
            {
                Errors = new List<Error>() { error };
            }
        }

        public OperationResult(bool result, List<Error> errors = null)
        {
            Result = result;
            Errors = errors;
        }

        public void AddError(Error error)
        {
            if (error == null)
            {
                return;
            }
            if (Errors == null)
            {
                Errors = new List<Error>();
            }
            Result = false;
            Errors.Add(error);
        }

        public void Merge(OperationResult result)
        {
            if (!result.Result)
            {
                foreach (Error error in result.Errors)
                {
                    AddError(error);
                }
            }
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Data { get; private set; }

        public OperationResult(bool result, T data, List<Error> errors = null) : base(result, errors)
        {
            Data = data;
        }
    }
}
