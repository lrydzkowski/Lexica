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

        public int NumOfCycles { get; private set; } = 2;

        public SetModeOperator SetOperator { get; private set; }

        public ModeTypeEnum ModeType { get; set; }

        public MaintainingSettings Settings { get; private set; }

        private Entry? CurrentEntry { get; set; } = null;

        public Dictionary<string, int> AnswersRegister { get; private set; } = new Dictionary<string, int>() { };

        public void Reset()
        {
            SetOperator.Reset();
            AnswersRegister = new Dictionary<string, int>();
        }

        public void Randomize() { }

        public int GetNumberOfQuestions()
        {
            return SetOperator.GetNumberOfEntries() * NumOfCycles;
        }

        public int GetResult()
        {
            return AnswersRegister.Select(x => x.Value).Sum();
        }

        public Question? GetQuestion(bool getCurrent = false, bool randomize = true)
        {
            if (!getCurrent)
            {
                if (IsAnswersRegisterCompleted())
                {
                    CurrentEntry = null;
                }
                else
                {
                    if (CurrentEntry == null && randomize)
                    {
                        SetOperator.Randomize();
                    }
                    Entry? entry = GetNextEntry();
                    int iteration = 0;
                    while (iteration < GetNumberOfQuestions() && entry != null && IsEntryCompleted(entry))
                    {
                        entry = GetNextEntry();
                        iteration++;
                    }
                    CurrentEntry = entry;
                }
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

        private bool IsAnswersRegisterCompleted()
        {
            if (GetNumberOfQuestions() != AnswersRegister.Count * NumOfCycles)
            {
                return false;
            }
            foreach (KeyValuePair<string, int> element in AnswersRegister)
            {
                if (element.Value < NumOfCycles)
                {
                    return false;
                }
            }
            return true;
        }

        private Entry? GetNextEntry()
        {
            Entry? entry = SetOperator.GetNextEntry();
            if (entry == null)
            {
                SetOperator.Reset();
                SetOperator.Randomize();
                entry = SetOperator.GetNextEntry();
            }
            return entry;
        }

        private bool IsEntryCompleted(Entry entry)
        {
            if (!AnswersRegister.ContainsKey(entry.Id) || AnswersRegister[entry.Id] < NumOfCycles)
            {
                return false;
            }
            return true;
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
            if (string.Join(',', answerWords) == string.Join(',', correctWords))
            {
                UpdateAnswersRegister(CurrentEntry, 1);
            }
            else
            {
                result = false;
                if (Settings.ResetAfterMistake == true)
                {
                    Reset();
                }
                else
                {
                    UpdateAnswersRegister(CurrentEntry, -1);
                }
            }

            return new AnswerResult(result, correctWords);
        }

        public void UpdateAnswersRegister(int value)
        {
            if (CurrentEntry == null)
            {
                return;
            }
            UpdateAnswersRegister(CurrentEntry, value);
        }

        private void UpdateAnswersRegister(Entry entry, int value)
        {
            if (!AnswersRegister.ContainsKey(entry.Id))
            {
                AnswersRegister[entry.Id] = 0;
            }
            AnswersRegister[entry.Id] += value;
        }
    }
}
