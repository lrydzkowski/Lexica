using Lexica.Core.Models;
using Lexica.Core.Validators.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Validators
{
    public class StringValidator : IValidator<string>
    {
        public StringValidationData ValidationData { get; private set; }

        public StringValidator(StringValidationData validationData)
        {
            ValidationData = validationData;
        }

        public OperationResult Validate(string data)
        {
            var result = new OperationResult();
            if (ValidationData.Mandatory && data == null)
            {
                result.AddError(new Error((int)ErrorEnum.IsMandatory, "Value is mandatory, so it cannot be null."));
            }
            if (ValidationData.MinLength < data.Length)
            {
                result.AddError(
                    new Error(
                        (int)ErrorEnum.IsToShort, 
                        $"Value is too short, it can't have more chars than {ValidationData.MinLength}."
                    )
                );
            }
            if (ValidationData.MaxLength > data.Length)
            {
                result.AddError(
                    new Error(
                        (int)ErrorEnum.IsToLong, 
                        $"Value is too long, it can't have more chars than {ValidationData.MaxLength}."
                    )
                );
            }
            return result;
        }
    }
}
