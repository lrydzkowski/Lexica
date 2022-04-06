using System.Collections.Generic;

namespace Lexica.Learning.Models
{
    public class AnswerResult
    {
        public bool Result { get; private set; }

        public List<string> GivenAnswers { get; private set; }

        public List<string> CorrectAnswers { get; private set; }

        public AnswerResult(bool result, List<string> givenAnswers, List<string> correctAnswers)
        {
            Result = result;
            GivenAnswers = givenAnswers;
            CorrectAnswers = correctAnswers;
        }
    }
}