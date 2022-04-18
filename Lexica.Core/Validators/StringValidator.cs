using System.Collections.Generic;
using Lexica.Core.Extensions;
using Lexica.Core.Models;
using Lexica.Core.Validators.Models;

namespace Lexica.Core.Validators
{
    public class StringValidator : IValidator<string?>
    {
        public StringValidationData ValidationData { get; }

        public StringValidator(StringValidationData validationData)
        {
            ValidationData = validationData;
        }

        public OperationResult Validate(string? data)
        {
            var result = new OperationResult();
            if (ValidationData.Mandatory && data == null)
            {
                result.AddError(
                    new Error(
                        (int)ErrorCodesEnum.IsMandatory,
                        $"{ValidationData.Name.UppercaseFirst()} is mandatory, so it cannot be null."
                    )
                );
            }
            if (data == null)
            {
                return result;
            }
            if (data.Length < ValidationData.MinLength)
            {
                result.AddError(
                    new Error<Dictionary<string, string>>(
                        (int)ErrorCodesEnum.IsTooShort,
                        $"Value '{data}' is too short, it can't have less chars than {ValidationData.MinLength}.",
                        new Dictionary<string, string>() { { "Name", ValidationData.Name }, { "Value", data } }
                    )
                );
            }
            if (data.Length > ValidationData.MaxLength)
            {
                result.AddError(
                    new Error<Dictionary<string, string>>(
                        (int)ErrorCodesEnum.IsTooLong,
                        $"Value '{data}' is too long, it can't have more chars than {ValidationData.MaxLength}.",
                        new Dictionary<string, string>() { { "Name", ValidationData.Name }, { "Value", data } }
                    )
                );
            }

            return result;
        }
    }
}