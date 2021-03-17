using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.MaintainingMode.Models
{
    public class AnswerRegister
    {
        public int PreviousValue { get; set; } = 0;

        public int CurrentValue { get; set; } = 0;
    }
}
