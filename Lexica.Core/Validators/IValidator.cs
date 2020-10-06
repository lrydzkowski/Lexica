using Lexica.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Validators
{
    public interface IValidator<T1>
    {
        public OperationResult Validate(T1 data);
    }
}
