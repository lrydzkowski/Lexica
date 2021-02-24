using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Validators.Models
{
    public class NumberValidationData : GeneralValidationData
    {
        public long MinValue { get; private set; }

        public long MaxValue { get; private set; }

        public NumberValidationData(long minValue, long maxValue, bool mandatory, string name) : base(mandatory, name)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
