using Lexica.Core.Models;
using Lexica.MaintainingMode.Models;
using Lexica.MaintainingMode.Config;
using Lexica.Words;
using Lexica.Words.Models;
using System.Collections.Generic;
using System.Linq;

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

        private int NumOfCycles { get; set; } = 2;

        private SetModeOperator SetOperator { get; set; }

        private ModeTypeEnum ModeType { get; set; }

        private MaintainingSettings Settings { get; set; }

        public Entry? CurrentEntry { get; private set; } = null;

        public Dictionary<string, int> AnswersRegister { get; private set; } = new Dictionary<string, int>() { };

        public void Reset()
        {
            AnswersRegister = new Dictionary<string, int>();
        }

        public int GetResult()
        {
            return AnswersRegister.Select(x => x.Value).Sum();
        }

        public IEnumerable<Question?> GetQuestions(bool randomizeEachIteration = true)
        {
            int numOfCompletedEntries = 0;
            foreach (Entry? entry in SetOperator.GetEntries(true, randomizeEachIteration))
            {
                if (entry == null)
                {
                    yield return null;
                }
                else if (IsEntryCompleted(entry))
                {
                    numOfCompletedEntries++;
                    if (numOfCompletedEntries == GetNumberOfQuestions())
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    CurrentEntry = entry;
                    List<string> questionWords = new List<string>();
                    switch (ModeType)
                    {
                        case ModeTypeEnum.Translations:
                            questionWords = entry.Words;
                            break;
                        case ModeTypeEnum.Words:
                            questionWords = entry.Translations;
                            break;
                    }
                    yield return new Question(string.Join(", ", questionWords), QuestionTypeEnum.Open);
                }
            }
        }

        public int GetNumberOfQuestions()
        {
            return SetOperator.GetNumberOfEntries() * NumOfCycles;
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
