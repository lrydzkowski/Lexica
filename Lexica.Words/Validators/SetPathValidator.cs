using Lexica.Core.Data;
using Lexica.Core.Models;
using Lexica.Core.Validators;
using Lexica.Words.Models;
using Lexica.Words.Validators.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Words.Validators
{
    class SetPathValidator : IValidator<SetPath>
    {
        public ValidationData ValidationData { get; private set; }

        public SetPathValidator(ValidationData validationData)
        {
            ValidationData = validationData;
        }

        public OperationResult Validate(SetPath setPath)
        {
            var result = new OperationResult();
            // Namespace validation.
            var namespaceValidator = new StringValidator(ValidationData.Namespace);
            result.Merge(namespaceValidator.Validate(setPath.Namespace));
            // Name validation.
            var nameValidator = new StringValidator(ValidationData.SetName);
            result.Merge(nameValidator.Validate(setPath.Name));

            return result;
        }
    }
}
