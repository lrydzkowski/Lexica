using System.Collections.Generic;

namespace Lexica.Learning.Models
{
    public class Question
    {
        public string Content { get; }

        public QuestionTypeEnum Type { get; }

        public List<string>? PossibleAnswers { get; }

        public Question(string content, QuestionTypeEnum type, List<string>? possibleAnswers = null)
        {
            Content = content;
            Type = type;
            PossibleAnswers = possibleAnswers;
        }
    }
}