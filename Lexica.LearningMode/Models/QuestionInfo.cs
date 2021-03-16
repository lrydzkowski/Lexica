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
            ModeTypeEnum modeType, 
            List<string>? possibleAnswers)
        {
            Entry = entry;
            QuestionType = questionType;
            ModeType = modeType;
            PossibleAnswers = possibleAnswers;
        }

        public Entry Entry { get; private set; }

        public QuestionTypeEnum QuestionType { get; private set; }

        public ModeTypeEnum ModeType { get; private set; }

        public List<string>? PossibleAnswers { get; private set; }

        public List<string> GetCorrectAnswers()
        {
            List<string> correctWords = new();
            switch (ModeType)
            {
                case ModeTypeEnum.Translations:
                    correctWords = Entry.Translations;
                    break;
                case ModeTypeEnum.Words:
                    correctWords = Entry.Words;
                    break;
            }
            correctWords.Sort();
            return correctWords;
        }
    }
}
