using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Config.Models
{
    public class Spelling
    {
        public bool ForvoAPI { get; set; }

        public int NumOfLevels { get; set; }

        public bool ResetAfterMistake { get; set; }
    }
}
