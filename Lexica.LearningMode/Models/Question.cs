using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.LearningMode.Models
{
    public class Question
    {
        public string Content { get; private set; }

        public QuestionTypeEnum Type { get; private set; }

        public List<string>? PossibleAnswers { get; private set; }

        public Question(string content, QuestionTypeEnum type, List<string>? possibleAnswers = null)
        {
            Content = content;
            Type = type;
            PossibleAnswers = possibleAnswers;
        }
    }
}
