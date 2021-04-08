using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.CLI.Modes.Learning.Models
{
    class ResultStatus
    {
        public int NumOfCorrectAnswers { get; set; }

        public int NumOfQuestions { get; set; }

        public override string ToString()
        {
            return $"{NumOfCorrectAnswers}/{NumOfQuestions}";
        }
    }
}
