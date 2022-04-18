using System.Collections.Generic;

namespace Lexica.Learning.Models
{
    public class AnswerResult
    {
        public bool Result { get; }

        public List<string> GivenAnswers { get; }

        public List<string> CorrectAnswers { get; }

        public AnswerResult(bool result, List<string> givenAnswers, List<string> correctAnswers)
        {
            Result = result;
            GivenAnswers = givenAnswers;
            CorrectAnswers = correctAnswers;
        }
    }
}