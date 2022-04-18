using System.Collections.Generic;
using Lexica.Core.Models;
using Lexica.Core.Validators.Models;

namespace Lexica.Core.Validators
{
    public class NumberValidator : IValidator<double?>
    {
        public NumberValidationData ValidationData { get; }

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
                    new Error<Dictionary<string, string>>(
                        (int)ErrorCodesEnum.IsMandatory,
                        "Value is mandatory, so it cannot be null.",
                        new Dictionary<string, string>() { { "Name", ValidationData.Name } }
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
                    new Error<Dictionary<string, string>>(
                        (int)ErrorCodesEnum.IsTooShort,
                        $"Value '{data}' is too small, it can't be smaller than {ValidationData.MinValue}.",
                        new Dictionary<string, string>() {
                            { "Name", ValidationData.Name }, { "Value", ((double)data).ToString() }
                        }
                    )
                );
            }
            if (data > ValidationData.MaxValue)
            {
                result.AddError(
                    new Error<Dictionary<string, string>>(
                        (int)ErrorCodesEnum.IsTooLong,
                        $"Value '{data}' is too big, it can't be bigger than {ValidationData.MaxValue}.",
                        new Dictionary<string, string>() {
                            { "Name", ValidationData.Name }, { "Value", ((double)data).ToString() }
                        }
                    )
                );
            }
            return result;
        }
    }
}