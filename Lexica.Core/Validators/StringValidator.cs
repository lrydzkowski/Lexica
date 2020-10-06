using Lexica.Core.Models;
using Lexica.Core.Models.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Validators
{
    public class StringValidator : IValidator<string>
    {
        public GeneralValidationData ValidationData { get; private set; }

        public StringValidator(GeneralValidationData validationData)
        {
            ValidationData = validationData;
        }

        public OperationResult Validate(string data)
        {
            var result = new OperationResult();
            if (ValidationData.Mandatory && data == null)
            {
                result.AddError(new Error("IsMandatory", "Value cannot be null."));
            }
            if (ValidationData.MinLength < data.Length)
            {
                result.AddError(new Error("IsToShort", $"Value cannot have more chars than {ValidationData.MinLength}."));
            }
            if (ValidationData.MaxLength > data.Length)
            {
                result.AddError(new Error("IsToLong", $"Value cannot have more chars than {ValidationData.MaxLength}."));
            }
            return result;
        }
    }
}
