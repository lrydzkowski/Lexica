using Lexica.Core.Models;
using Lexica.Core.Validators.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Validators
{
    public class NumberValidator : IValidator<double?>
    {
        public NumberValidationData ValidationData { get; private set; }

        public NumberValidator(NumberValidationData validationData)
        {
            ValidationData = validationData;
        }

        public OperationResult Validate(double? data)
        {
            var result = new OperationResult();
            if (ValidationData.Mandatory && data == null)
            {
                result.AddError(
                    new Error(
                        (int)ErrorCodesEnum.IsMandatory,
                        "Value is mandatory, so it cannot be null."
                    )
                );
            }
            if (data == null)
            {
                return result;
            }
            if (data < ValidationData.MinValue)
            {
                result.AddError(
                    new Error(
                        (int)ErrorCodesEnum.IsTooShort,
                        $"Value is too small, it can't be smaller than {ValidationData.MinValue}."
                    )
                );
            }
            if (data > ValidationData.MaxValue)
            {
                result.AddError(
                    new Error(
                        (int)ErrorCodesEnum.IsTooLong,
                        $"Value is too big, it can't be bigger than {ValidationData.MaxValue}."
                    )
                );
            }
            return result;
        }
    }
}
