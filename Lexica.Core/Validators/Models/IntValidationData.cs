using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Validators.Models
{
    public class IntValidationData : GeneralValidationData
    {
        public int MinSize { get; private set; }

        public int MaxSize { get; private set; }

        public IntValidationData(int minSize, int maxSize, bool mandatory) : base(mandatory)
        {
            MinSize = minSize;
            MaxSize = maxSize;
        }
    }
}
