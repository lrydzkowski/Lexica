using Lexica.Core.Config;
using Lexica.Core.Models;
using Lexica.MaintainingMode.Config.Models;
using Lexica.Words;
using Lexica.Words.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.MaintainingMode
{
    public class Manager
    {
        public Manager(SetModeOperator setOperator, AppSettings<Maintaining> settings)
        {
            SetOperator = setOperator;
            Settings = settings;
        }

        public SetModeOperator SetOperator { get; private set; }

        public AppSettings<Maintaining> Settings { get; private set; }

        private Entry? CurrentEntry { get; set; } = null;

        public async Task<Question?> GetQuestion(bool getCurrent = false)
        {
            if (!getCurrent)
            {
                Entry? entry = await SetOperator.GetNextEntry();
                CurrentEntry = entry;
            }
            if (CurrentEntry == null)
            {
                return null;
            }
            return new Question(string.Join(", ", CurrentEntry.Words), QuestionTypeEnum.Open);
        }

        public AnswerResult? VerifyAnswer(string input)
        {
            if (CurrentEntry == null)
            {
                return null;
            }
            List<string> answerWords = input.Split(',').Select(x => x.Trim()).ToList<string>();
            answerWords.Sort();
            List<string> correctWords = CurrentEntry.Translations;
            correctWords.Sort();

            bool result = true;
            if (string.Join(',', answerWords) != string.Join(',', correctWords))
            {
                result = false;
            }

            return new AnswerResult(result, correctWords);
        }
    }
}
