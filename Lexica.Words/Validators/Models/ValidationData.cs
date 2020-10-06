using Lexica.Core.Models.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Words.Validators.Models
{
    class ValidationData
    {
        public GeneralValidationData Namespace { get; set; }

        public GeneralValidationData SetName { get; set; }

        public GeneralValidationData EntryWord { get; set; }

        public GeneralValidationData EntryTranslation { get; set; }
    }
}
