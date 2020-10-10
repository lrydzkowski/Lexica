using Lexica.Core.Validators.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Words.Validators.Models
{
    public class ValidationData
    {
        public StringValidationData Namespace { get; set; } = new StringValidationData(
            minLength: 1, maxLength: 400, mandatory: true
        );

        public StringValidationData SetName { get; set; } = new StringValidationData(
            minLength: 1, maxLength: 100, mandatory: true
        );

        public StringValidationData EntryWord { get; set; } = new StringValidationData(
            minLength: 1, maxLength: 50, mandatory: true
        );

        public StringValidationData EntryTranslation { get; set; } = new StringValidationData(
            minLength: 1, maxLength: 50, mandatory: true
        );
    }
}
