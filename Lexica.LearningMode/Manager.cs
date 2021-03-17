using Lexica.Core.Models;
using Lexica.LearningMode.Config;
using Lexica.LearningMode.Models;
using Lexica.Words;
using Lexica.Words.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lexica.LearningMode
{
    public class Manager
    {
        public Manager(SetModeOperator setOperator, LearningSettings settings)
        {
            SetOperator = setOperator;
            Settings = settings;
            Reset();
        }

        private SetModeOperator SetOperator { get; set; }

        private LearningSettings Settings { get; set; }

        public QuestionInfo? CurrentQuestionInfo { get; private set; } = null;

        public Dictionary<QuestionTypeEnum, Dictionary<string, AnswerRegister>> AnswersRegister { get; private set; }

        public void Reset()
        {
            AnswersRegister = new Dictionary<QuestionTypeEnum, Dictionary<string, AnswerRegister>>();
            AnswersRegister[QuestionTypeEnum.Closed] = new Dictionary<string, AnswerRegister>();
            AnswersRegister[QuestionTypeEnum.Open] = new Dictionary<string, AnswerRegister>();
        }

        public Tuple<int, int> GetResult()
        {
            int closedResult = AnswersRegister[QuestionTypeEnum.Closed]
                .Select(x => x.Value.Translations.CurrentValue + x.Value.Words.CurrentValue)
                .Sum();
            int openResult = AnswersRegister[QuestionTypeEnum.Open]
                .Select(x => x.Value.Translations.CurrentValue + x.Value.Words.CurrentValue)
                .Sum();
            return new Tuple<int, int>(closedResult, openResult);
        }

        public IEnumerable<Question?> GetQuestions(bool randomizeEachIteration = true)
        {
            int numOfCompletedEntries = 0;
            foreach (Entry? entry in SetOperator.GetEntries(true, randomizeEachIteration, 7))
            {
                if (entry == null)
                {
                    yield return null;
                }
                else if (IsEntryQuestionsCompleted(entry))
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
                    numOfCompletedEntries = 0;
                    bool translationsQuestion = false;
                    bool wordsQuestion = false;
                    QuestionTypeEnum questionType = QuestionTypeEnum.Closed;
                    List<string> questionWords = new();
                    ModeTypeEnum modeType = ModeTypeEnum.Translations;
                    List<string>? possibleAnswers = null;
                    if (!IsEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed)) // pytanie zamknięte
                    {
                        questionType = QuestionTypeEnum.Closed;
                        var rnd = new Random();
                        if (    !IsEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed, ModeTypeEnum.Translations)
                            &&  !IsEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed, ModeTypeEnum.Words))
                        {
                            ModeTypeEnum closedQuestionMode = rnd.Next(1) == 1
                                ? ModeTypeEnum.Translations
                                : ModeTypeEnum.Words;
                            if (closedQuestionMode == ModeTypeEnum.Translations)
                            {
                                translationsQuestion = true;
                            }
                            else
                            {
                                wordsQuestion = true;
                            }
                        }
                        else if (!IsEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed, ModeTypeEnum.Translations))
                        {
                            translationsQuestion = true;
                        }
                        else if (!IsEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed, ModeTypeEnum.Words))
                        {
                            wordsQuestion = true;
                        }
                        if (translationsQuestion)
                        {
                            modeType = ModeTypeEnum.Translations;
                            questionWords = entry.Translations;
                            possibleAnswers = SetOperator.GetRandomEntries(4)
                                .Select(x => string.Join(", ", x.Words))
                                .ToList();
                            string properAnswer = string.Join(", ", entry.Words);
                            if (!possibleAnswers.Contains(properAnswer))
                            {
                                possibleAnswers[rnd.Next(0, 4)] = string.Join(", ", entry.Words);
                            }
                        }
                        else if (wordsQuestion)
                        {
                            modeType = ModeTypeEnum.Words;
                            questionWords = entry.Words;
                            possibleAnswers = SetOperator.GetRandomEntries(4)
                                .Select(x => string.Join(", ", x.Translations))
                                .ToList();
                            string properAnswer = string.Join(", ", entry.Translations);
                            if (!possibleAnswers.Contains(properAnswer))
                            {
                                possibleAnswers[rnd.Next(0, 4)] = string.Join(", ", entry.Translations);
                            }
                        }
                    }
                    else if (!IsEntryQuestionsCompleted(entry, QuestionTypeEnum.Open)) // pytanie otwarte
                    {
                        questionType = QuestionTypeEnum.Open;
                        if (    !IsEntryQuestionsCompleted(entry, QuestionTypeEnum.Open, ModeTypeEnum.Translations)
                            &&  !IsEntryQuestionsCompleted(entry, QuestionTypeEnum.Open, ModeTypeEnum.Words))
                        {
                            var rnd = new Random();
                            ModeTypeEnum closedQuestionMode = rnd.Next(1) == 1
                                ? ModeTypeEnum.Translations
                                : ModeTypeEnum.Words;
                            if (closedQuestionMode == ModeTypeEnum.Translations)
                            {
                                translationsQuestion = true;
                            }
                            else
                            {
                                wordsQuestion = true;
                            }
                        }
                        else if (!IsEntryQuestionsCompleted(entry, QuestionTypeEnum.Open, ModeTypeEnum.Translations))
                        {
                            translationsQuestion = true;
                        }
                        else if (!IsEntryQuestionsCompleted(entry, QuestionTypeEnum.Open, ModeTypeEnum.Words))
                        {
                            wordsQuestion = true;
                        }
                        if (translationsQuestion)
                        {
                            modeType = ModeTypeEnum.Translations;
                            questionWords = entry.Translations;
                        }
                        else if (wordsQuestion)
                        {
                            modeType = ModeTypeEnum.Words;
                            questionWords = entry.Words;
                        }
                    }

                    CurrentQuestionInfo = new QuestionInfo(entry, questionType, modeType, possibleAnswers);

                    yield return new Question(string.Join(", ", questionWords), questionType, possibleAnswers);
                }
            }
        }

        public int GetNumberOfQuestions()
        {
            return GetNumberOfQuestions(QuestionTypeEnum.Closed) + GetNumberOfQuestions(QuestionTypeEnum.Open);
        }

        public int GetNumberOfQuestions(QuestionTypeEnum questionType)
        {
            return SetOperator.GetNumberOfEntries() * (GetNumOfRequiredAnswers(questionType) * 2);
        }

        private bool IsEntryQuestionsCompleted(Entry entry)
        {
            return IsEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed) 
                && IsEntryQuestionsCompleted(entry, QuestionTypeEnum.Open);
        }

        private bool IsEntryQuestionsCompleted(Entry entry, QuestionTypeEnum questionTypeEnum)
        {
            return IsEntryQuestionsCompleted(entry, questionTypeEnum, ModeTypeEnum.Translations)
                && IsEntryQuestionsCompleted(entry, questionTypeEnum, ModeTypeEnum.Words);
        }

        private bool IsEntryQuestionsCompleted(Entry entry, QuestionTypeEnum questionTypeEnum, ModeTypeEnum mode)
        {
            if (!AnswersRegister[questionTypeEnum].ContainsKey(entry.Id))
            {
                return false;
            }
            string modeTypeKey = mode.ToString("g");
            int currentValue = AnswersRegister[questionTypeEnum][entry.Id][modeTypeKey].CurrentValue;
            if (currentValue < GetNumOfRequiredAnswers(questionTypeEnum))
            {
                return false;
            }
            return true;
        }

        public int GetNumOfRequiredAnswers(QuestionTypeEnum questionType)
        {
            int numOfRequiredAnswers = 0;
            switch (questionType)
            {
                case QuestionTypeEnum.Closed:
                    return 1;
                case QuestionTypeEnum.Open:
                    return Settings.NumOfLevels ?? 2;
            }
            return numOfRequiredAnswers;
        }

        public AnswerResult? VerifyAnswer(string input)
        {
            if (CurrentQuestionInfo == null)
            {
                return null;
            }
            if (CurrentQuestionInfo.QuestionType == QuestionTypeEnum.Closed)
            {
                bool parsingResult = int.TryParse(input, out int optionIndex);
                optionIndex--;
                input = "";
                if (parsingResult && CurrentQuestionInfo.PossibleAnswers != null)
                {
                    if (optionIndex < CurrentQuestionInfo.PossibleAnswers.Count)
                    {
                        input = CurrentQuestionInfo.PossibleAnswers[optionIndex];
                    }
                }
            }
            List<string> answerWords = input.Split(',').Select(x => x.Trim()).ToList<string>();
            answerWords.Sort();
            List<string> correctWords = CurrentQuestionInfo.GetCorrectAnswers();
            correctWords.Sort();

            bool result = true;
            if (string.Join(',', answerWords) == string.Join(',', correctWords))
            {
                UpdateAnswersRegister(1);
            }
            else
            {
                result = false;
                UpdateAnswersRegister(0, UpdateAnswersRegisterOperationType.Set);
            }

            return new AnswerResult(result, correctWords);
        }

        public void UpdateAnswersRegister(
            int value, 
            UpdateAnswersRegisterOperationType operationType = UpdateAnswersRegisterOperationType.Add)
        {
            if (CurrentQuestionInfo == null)
            {
                return;
            }
            UpdateAnswersRegister(CurrentQuestionInfo, value, operationType);
        }

        private void UpdateAnswersRegister(
            QuestionInfo questionInfo, 
            int value,
            UpdateAnswersRegisterOperationType operationType = UpdateAnswersRegisterOperationType.Add)
        {
            string modeTypeKey = questionInfo.ModeType.ToString("g");
            if (!AnswersRegister[questionInfo.QuestionType].ContainsKey(questionInfo.Entry.Id))
            {
                AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id] = new AnswerRegister();
                AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][modeTypeKey] = new AnswerRegisterValue();
            }
            switch (operationType)
            {
                case UpdateAnswersRegisterOperationType.Add:
                    AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][modeTypeKey].PreviousValue
                        += AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][modeTypeKey].CurrentValue;
                    AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][modeTypeKey].CurrentValue 
                        += value;
                    break;
                case UpdateAnswersRegisterOperationType.Set:
                    AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][modeTypeKey].PreviousValue
                        += AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][modeTypeKey].CurrentValue;
                    AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][modeTypeKey].CurrentValue 
                        = value;
                    break;
            }
        }

        public void OverridePreviousMistake()
        {
            if (CurrentQuestionInfo == null)
            {
                return;
            }
            QuestionInfo questionInfo = CurrentQuestionInfo;
            string modeTypeKey = questionInfo.ModeType.ToString("g");
            AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][modeTypeKey].CurrentValue
                = AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][modeTypeKey].PreviousValue + 1;
        }
    }
}
