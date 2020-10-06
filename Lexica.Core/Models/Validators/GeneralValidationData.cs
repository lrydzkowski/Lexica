using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lexica.Core.Models.Validators
{
    public class GeneralValidationData
    {
        public int MinLength { get; private set; }

        public int MaxLength { get; private set; }

        public bool Mandatory { get; private set; }

        public GeneralValidationData(int minLength, int maxLength, bool mandatory)
        {
            MinLength = minLength;
            MaxLength = maxLength;
            Mandatory = mandatory;
        }
    }
}
