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
        public Manager(SetModeOperator setOperator, LearningSettings settings, ModeEnum mode)
        {
            SetOperator = setOperator;
            Settings = settings;
            Mode = mode;
            Reset();
        }

        private SetModeOperator SetOperator { get; set; }

        private LearningSettings Settings { get; set; }

        private ModeEnum Mode { get; set; }

        public QuestionInfo? CurrentQuestionInfo { get; private set; } = null;

        public Dictionary<QuestionTypeEnum, Dictionary<string, AnswerRegister>> AnswersRegister { get; private set; } 
            = new Dictionary<QuestionTypeEnum, Dictionary<string, AnswerRegister>>();

        public void Reset()
        {
            AnswersRegister[QuestionTypeEnum.Closed] = new Dictionary<string, AnswerRegister>();
            AnswersRegister[QuestionTypeEnum.Open] = new Dictionary<string, AnswerRegister>();
        }

        public Tuple<int, int> GetResult()
        {
            return new Tuple<int, int>(GetResult(QuestionTypeEnum.Closed), GetResult(QuestionTypeEnum.Open));
        }

        public int GetResult(QuestionTypeEnum questionType)
        {
            return AnswersRegister[questionType]
                .Select(x => x.Value.Translations.CurrentValue + x.Value.Words.CurrentValue)
                .Sum();
        }

        public int GetSumResult()
        {
            (int closedResult, int openResult) = GetResult();
            return closedResult + openResult;
        }

        public IEnumerable<Question?> GetQuestions(bool randomizeEachIteration = true, int pieceSize = 8)
        {
            foreach (Entry? entry in SetOperator.GetEntries(true, randomizeEachIteration, pieceSize))
            {
                if (entry == null)
                {
                    yield return null;
                }
                else if (AreEntryQuestionsCompleted(entry))
                {
                    if (AreAllQuestionsCompleted())
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
                    QuestionTypeEnum questionType = QuestionTypeEnum.Closed;
                    List<string> questionWords = new();
                    AnswerTypeEnum answerType = AnswerTypeEnum.Translations;
                    List<string>? possibleAnswers = null;
                    if (!AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed)) // pytanie zamknięte
                    {
                        questionType = QuestionTypeEnum.Closed;
                        var rnd = new Random();
                        if (    !AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed, AnswerTypeEnum.Translations)
                            &&  !AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed, AnswerTypeEnum.Words))
                        {
                            answerType = rnd.Next(2) == 1 ? AnswerTypeEnum.Translations : AnswerTypeEnum.Words;
                        }
                        else if (!AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed, AnswerTypeEnum.Words))
                        {
                            answerType = AnswerTypeEnum.Words;
                        }
                        else if (!AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed, AnswerTypeEnum.Translations))
                        {
                            answerType = AnswerTypeEnum.Translations;
                        }
                        switch (answerType)
                        {
                            case AnswerTypeEnum.Words:
                                questionWords = entry.Translations;
                                possibleAnswers = SetOperator.GetRandomEntries(4)
                                    .Select(x => string.Join(", ", x.Words))
                                    .ToList();
                                string wordsProperAnswer = string.Join(", ", entry.Words);
                                if (!possibleAnswers.Contains(wordsProperAnswer))
                                {
                                    possibleAnswers[rnd.Next(0, 4)] = string.Join(", ", entry.Words);
                                }
                                break;
                            case AnswerTypeEnum.Translations:
                                questionWords = entry.Words;
                                possibleAnswers = SetOperator.GetRandomEntries(4)
                                    .Select(x => string.Join(", ", x.Translations))
                                    .ToList();
                                string translationsProperAnswer = string.Join(", ", entry.Translations);
                                if (!possibleAnswers.Contains(translationsProperAnswer))
                                {
                                    possibleAnswers[rnd.Next(0, 4)] = string.Join(", ", entry.Translations);
                                }
                                break;
                        }
                    }
                    else if (!AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Open)) // pytanie otwarte
                    {
                        questionType = QuestionTypeEnum.Open;
                        if (    !AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Open, AnswerTypeEnum.Translations)
                            &&  !AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Open, AnswerTypeEnum.Words))
                        {
                            var rnd = new Random();
                            answerType = rnd.Next(2) == 1 ? AnswerTypeEnum.Translations : AnswerTypeEnum.Words;
                        }
                        else if (!AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Open, AnswerTypeEnum.Words))
                        {
                            answerType = AnswerTypeEnum.Words;
                        }
                        else if (!AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Open, AnswerTypeEnum.Translations))
                        {
                            answerType = AnswerTypeEnum.Translations;
                        }
                        switch (answerType)
                        {
                            case AnswerTypeEnum.Words:
                                questionWords = entry.Translations;
                                break;
                            case AnswerTypeEnum.Translations:
                                questionWords = entry.Words;
                                if (Mode == ModeEnum.Spelling)
                                {
                                    answerType = AnswerTypeEnum.Words;
                                }
                                break;
                        }
                    }

                    var nextQuestionInfo = new QuestionInfo(entry, questionType, answerType, possibleAnswers);
                    // Condition to prevent questions repetition
                    if (!IsCurrentQuestionTheLastOne() && nextQuestionInfo.Equals(CurrentQuestionInfo))
                    {
                        continue;
                    }

                    CurrentQuestionInfo = nextQuestionInfo;

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
            int multiplier = 2;
            if (Mode == ModeEnum.Spelling)
            {
                multiplier = 1;
            }
            return SetOperator.GetNumberOfEntries() * (GetNumOfRequiredAnswers(questionType) * multiplier);
        }

        private bool IsCurrentQuestionTheLastOne()
        {
            return GetNumberOfQuestions() - GetSumResult() == 1;
        }

        private bool AreAllQuestionsCompleted()
        {
            return GetNumberOfQuestions() == GetSumResult();
        }

        private bool AreEntryQuestionsCompleted(Entry entry)
        {
            return AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed) 
                && AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Open);
        }

        private bool AreEntryQuestionsCompleted(Entry entry, QuestionTypeEnum questionTypeEnum)
        {
            return AreEntryQuestionsCompleted(entry, questionTypeEnum, AnswerTypeEnum.Translations)
                && AreEntryQuestionsCompleted(entry, questionTypeEnum, AnswerTypeEnum.Words);
        }

        private bool AreEntryQuestionsCompleted(
            Entry entry,
            QuestionTypeEnum questionTypeEnum,
            AnswerTypeEnum answerType)
        {
            if (Mode != ModeEnum.Full && questionTypeEnum == QuestionTypeEnum.Closed)
            {
                return true;
            }
            if (Mode == ModeEnum.Spelling && answerType == AnswerTypeEnum.Translations)
            {
                return true;
            }
            if (!AnswersRegister[questionTypeEnum].ContainsKey(entry.Id))
            {
                return false;
            }
            string answerTypeKey = answerType.ToString("g");
            int currentValue = AnswersRegister[questionTypeEnum][entry.Id][answerTypeKey].CurrentValue;
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
                    if (Mode == ModeEnum.Full)
                    {
                        numOfRequiredAnswers = 1;
                    }
                    break;
                case QuestionTypeEnum.Open:
                    numOfRequiredAnswers = Settings.NumOfLevels;
                    break;
                default:
                    break;
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
                UpdateAnswersRegister(0, UpdateAnswersRegisterOperationTypeEnum.Set);
            }

            return new AnswerResult(result, correctWords);
        }

        public void UpdateAnswersRegister(
            int value, 
            UpdateAnswersRegisterOperationTypeEnum operationType = UpdateAnswersRegisterOperationTypeEnum.Add)
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
            UpdateAnswersRegisterOperationTypeEnum operationType = UpdateAnswersRegisterOperationTypeEnum.Add)
        {
            string answerTypeKey = questionInfo.AnswerType.ToString("g");
            if (!AnswersRegister[questionInfo.QuestionType].ContainsKey(questionInfo.Entry.Id))
            {
                AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id] = new AnswerRegister();
                AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][answerTypeKey] = new AnswerRegisterValue();
            }
            switch (operationType)
            {
                case UpdateAnswersRegisterOperationTypeEnum.Add:
                    AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][answerTypeKey].PreviousValue
                        += AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][answerTypeKey].CurrentValue;
                    AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][answerTypeKey].CurrentValue 
                        += value;
                    break;
                case UpdateAnswersRegisterOperationTypeEnum.Set:
                    AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][answerTypeKey].PreviousValue
                        += AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][answerTypeKey].CurrentValue;
                    AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][answerTypeKey].CurrentValue 
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
            string answerTypeKey = questionInfo.AnswerType.ToString("g");
            AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][answerTypeKey].CurrentValue
                = AnswersRegister[questionInfo.QuestionType][questionInfo.Entry.Id][answerTypeKey].PreviousValue + 1;
        }
    }
}
