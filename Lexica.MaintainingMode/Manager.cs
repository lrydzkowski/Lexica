using Lexica.Core.Models;
using Lexica.MaintainingMode.Models;
using Lexica.MaintainingMode.Config;
using Lexica.Words;
using Lexica.Words.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lexica.MaintainingMode
{
    public class Manager
    {
        public Manager(SetModeOperator setOperator, ModeTypeEnum modeType, MaintainingSettings settings)
        {
            SetOperator = setOperator;
            ModeType = modeType;
            Settings = settings;
        }

        public SetModeOperator SetOperator { get; private set; }

        public ModeTypeEnum ModeType { get; set; }

        public MaintainingSettings Settings { get; private set; }

        private Entry? CurrentEntry { get; set; } = null;

        public async Task Randomize()
        {
            await SetOperator.Randomize();
        }

        public void Reset()
        {
            SetOperator.Reset();
        }

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
            List<string> questionWords = new List<string>();
            switch (ModeType)
            {
                case ModeTypeEnum.Translations:
                    questionWords = CurrentEntry.Words;
                    break;
                case ModeTypeEnum.Words:
                    questionWords = CurrentEntry.Translations;
                    break;
            }

            return new Question(string.Join(", ", questionWords), QuestionTypeEnum.Open);
        }

        public AnswerResult? VerifyAnswer(string input)
        {
            if (CurrentEntry == null)
            {
                return null;
            }
            List<string> answerWords = input.Split(',').Select(x => x.Trim()).ToList<string>();
            answerWords.Sort();
            List<string> correctWords = new List<string>();
            switch (ModeType)
            {
                case ModeTypeEnum.Translations:
                    correctWords = CurrentEntry.Translations;
                    break;
                case ModeTypeEnum.Words:
                    correctWords = CurrentEntry.Words;
                    break;
            }
            correctWords.Sort();

            bool result = true;
            if (string.Join(',', answerWords) != string.Join(',', correctWords))
            {
                result = false;
                if (Settings.ResetAfterMistake == true)
                {
                    SetOperator.Reset();
                }
            }

            return new AnswerResult(result, correctWords);
        }
    }
}
