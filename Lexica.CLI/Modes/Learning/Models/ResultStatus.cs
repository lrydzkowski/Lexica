using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.CLI.Modes.Learning.Models
{
    class ResultStatus
    {
        public ResultStatus(int numOfCorrectAnswers, int numOfQuestions)
        {
            NumOfCorrectAnswers = numOfCorrectAnswers;
            NumOfQuestions = numOfQuestions;
        }

        public int NumOfCorrectAnswers { get; private set; }

        public int NumOfQuestions { get; private set; }

        public override string ToString()
        {
            return $"{NumOfCorrectAnswers}/{NumOfQuestions}";
        }

        public string ToString(int leftPad)
        {
            return NumOfCorrectAnswers.ToString().PadLeft(leftPad) + "/" + NumOfQuestions;
        }
    }
}
