using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Models
{
    public class OperationResult
    {
        public bool Result { get; private set; }

        public List<Error>? Errors { get; private set; }

        public OperationResult()
        {
            Result = true;
        }

        public OperationResult(bool result)
        {
            Result = result;
        }

        public OperationResult(bool result, Error? error)
        {
            Result = result;
            if (error != null)
            {
                Errors = new List<Error>() { error };
            }
        }

        public OperationResult(bool result, List<Error>? errors = null)
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
            if (!result.Result && result.Errors != null)
            {
                foreach (Error error in result.Errors)
                {
                    AddError(error);
                }
            }
        }

        public void AddDictionaryDataToError(Dictionary<string, string> data)
        {
            if (Errors == null)
            {
                return;
            }
            for (int i = 0; i < Errors.Count; i++)
            {
                if (Errors[i] is Error<Dictionary<string, string>> error)
                {
                    foreach (KeyValuePair<string, string> el in data)
                    {
                        error.Data[el.Key] = el.Value;
                    }
                }
            }
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Data { get; private set; }

        public OperationResult(bool result, T data, List<Error>? errors = null) : base(result, errors)
        {
            Data = data;
        }

        public void Merge(OperationResult<T> result, MergeModeEnum mergeMode)
        {
            base.Merge(result);
            switch (mergeMode)
            {
                case MergeModeEnum.Overwrite:
                    Data = result.Data;
                    break;
                case MergeModeEnum.OverwriteIfNull:
                    if (Data == null)
                    {
                        Data = result.Data;
                    }
                    break;
            }
        }
    }
}
