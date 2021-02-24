using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lexica.Core.Validators.Models
{
    public class GeneralValidationData
    {
        public bool Mandatory { get; private set; }

        public string Name { get; private set; }

        public GeneralValidationData(bool mandatory, string name)
        {
            Mandatory = mandatory;
            Name = name;
        }
    }
}
