using Lexica.Core.Models;
using Lexica.Words.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.LearningMode.Models
{
    public class QuestionInfo
    {
        public QuestionInfo(
            Entry entry, 
            QuestionTypeEnum questionType, 
            AnswerTypeEnum answerType, 
            List<string>? possibleAnswers)
        {
            Entry = entry;
            QuestionType = questionType;
            AnswerType = answerType;
            PossibleAnswers = possibleAnswers;
        }

        public Entry Entry { get; private set; }

        public QuestionTypeEnum QuestionType { get; private set; }

        public AnswerTypeEnum AnswerType { get; private set; }

        public List<string>? PossibleAnswers { get; private set; }

        public List<string> GetCorrectAnswers()
        {
            List<string> correctWords = new();
            switch (AnswerType)
            {
                case AnswerTypeEnum.Translations:
                    correctWords = Entry.Translations;
                    break;
                case AnswerTypeEnum.Words:
                    correctWords = Entry.Words;
                    break;
            }
            correctWords.Sort();
            return correctWords;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                var questionInfo = (QuestionInfo)obj;
                return questionInfo.Entry.Equals(Entry)
                    && questionInfo.QuestionType == QuestionType
                    && questionInfo.AnswerType == AnswerType
                    && ListsEquals(questionInfo.PossibleAnswers, PossibleAnswers);
            }
        }

        private bool ListsEquals(List<string>? list1, List<string>? list2)
        {
            list1 ??= new List<string>();
            list2 ??= new List<string>();
            IEnumerable<string> isFirstOnly = list1.Except(list2);
            IEnumerable<string> isSecondOnly = list2.Except(list1);
            return !isFirstOnly.Any() && !isSecondOnly.Any();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
