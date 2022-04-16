using System;
using System.Collections.Generic;
using System.Linq;
using Lexica.Learning.Config;
using Lexica.Learning.Models;
using Lexica.Words;
using Lexica.Words.Models;

namespace Lexica.Learning
{
    public class LearningModeOperator
    {
        private readonly Random randomGenerator = new();

        public LearningModeOperator(WordsSetOperator wordsSetOperator, LearningSettings settings, ModeEnum mode)
        {
            WordsSetOperator = wordsSetOperator;
            Settings = settings;
            Mode = mode;
            Reset();
        }

        private WordsSetOperator WordsSetOperator { get; }

        private LearningSettings Settings { get; }

        private ModeEnum Mode { get; }

        public QuestionInfo? CurrentQuestionInfo { get; private set; } = null;

        public Dictionary<QuestionTypeEnum, Dictionary<string, AnswerRegister>> AnswersRegister { get; }
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

        public int GetCurrentQuestionResult(QuestionTypeEnum? questionType = null)
        {
            if (CurrentQuestionInfo == null)
            {
                return 0;
            }
            if (questionType == null)
            {
                questionType = CurrentQuestionInfo.QuestionType;
            }
            if (!AnswersRegister[(QuestionTypeEnum)questionType].ContainsKey(CurrentQuestionInfo.Entry.Id))
            {
                return 0;
            }
            return AnswersRegister[(QuestionTypeEnum)questionType]
                [CurrentQuestionInfo.Entry.Id]
                [CurrentQuestionInfo.AnswerType.ToString()]
                .CurrentValue;
        }

        public int GetSumResult()
        {
            (int closedResult, int openResult) = GetResult();
            return closedResult + openResult;
        }

        public IEnumerable<Question?> GetQuestions(bool randomizeEachIteration = true, int pieceSize = 7)
        {
            foreach (Entry? entry in WordsSetOperator.GetEntries(true, randomizeEachIteration, pieceSize))
            {
                if (entry == null)
                {
                    yield return null;
                    continue;
                }
                if (AreEntryQuestionsCompleted(entry))
                {
                    if (AreAllQuestionsCompleted())
                    {
                        break;
                    }
                    continue;
                }
                QuestionTypeEnum questionType = QuestionTypeEnum.Closed;
                List<string> questionWords = new();
                AnswerTypeEnum answerType = AnswerTypeEnum.Translations;
                List<string>? possibleAnswers = null;
                if (!AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed)) // close question
                {
                    questionType = QuestionTypeEnum.Closed;
                    if (!AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed, AnswerTypeEnum.Translations)
                        && !AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed, AnswerTypeEnum.Words))
                    {
                        answerType = randomGenerator.Next(2) == 1
                            ? AnswerTypeEnum.Translations
                            : AnswerTypeEnum.Words;
                    }
                    else if (!AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed, AnswerTypeEnum.Words))
                    {
                        answerType = AnswerTypeEnum.Words;
                    }
                    else if (!AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Closed, AnswerTypeEnum.Translations))
                    {
                        answerType = AnswerTypeEnum.Translations;
                    }
                    int numOfPossibleAnswers = 4;
                    switch (answerType)
                    {
                        case AnswerTypeEnum.Words:
                            questionWords = entry.Translations;
                            possibleAnswers = WordsSetOperator.GetRandomEntries(numOfPossibleAnswers)
                                .ConvertAll(x => string.Join(", ", x.Words));
                            string wordsProperAnswer = string.Join(", ", entry.Words);
                            if (!possibleAnswers.Contains(wordsProperAnswer))
                            {
                                possibleAnswers[randomGenerator.Next(0, numOfPossibleAnswers)] = string.Join(
                                    ", ", entry.Words
                                );
                            }
                            break;

                        case AnswerTypeEnum.Translations:
                            questionWords = entry.Words;
                            possibleAnswers = WordsSetOperator.GetRandomEntries(numOfPossibleAnswers)
                                .ConvertAll(x => string.Join(", ", x.Translations));
                            string translationsProperAnswer = string.Join(", ", entry.Translations);
                            if (!possibleAnswers.Contains(translationsProperAnswer))
                            {
                                possibleAnswers[randomGenerator.Next(0, numOfPossibleAnswers)] = string.Join(
                                    ", ", entry.Translations
                                );
                            }
                            break;
                    }
                }
                else if (!AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Open)) // open question
                {
                    questionType = QuestionTypeEnum.Open;
                    if (!AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Open, AnswerTypeEnum.Translations)
                        && !AreEntryQuestionsCompleted(entry, QuestionTypeEnum.Open, AnswerTypeEnum.Words))
                    {
                        answerType = randomGenerator.Next(2) == 1
                            ? AnswerTypeEnum.Translations
                            : AnswerTypeEnum.Words;
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
                if (!CanCurrentQuestionBeTheLastOne() && IsQuestionRepeated(nextQuestionInfo))
                {
                    continue;
                }
                CurrentQuestionInfo = nextQuestionInfo;

                yield return new Question(string.Join(", ", questionWords), questionType, possibleAnswers);
            }
        }

        public int GetNumberOfQuestions()
        {
            return GetNumberOfQuestions(QuestionTypeEnum.Closed) + GetNumberOfQuestions(QuestionTypeEnum.Open);
        }

        public int GetNumberOfQuestions(QuestionTypeEnum questionType)
        {
            return WordsSetOperator.GetNumberOfEntries() * GetEntryNumberOfQuestions(questionType);
        }

        public int GetEntryNumberOfQuestions(QuestionTypeEnum questionType)
        {
            return GetNumOfRequiredAnswers(questionType) * GetNumOfRequiredAnswersMultiplier();
        }

        public int GetNumberOfCurrentQuestions()
        {
            if (CurrentQuestionInfo == null)
            {
                return 0;
            }
            return GetNumOfRequiredAnswers(CurrentQuestionInfo.QuestionType);
        }

        public int GetNumOfRequiredAnswersMultiplier()
        {
            int multiplier = Enum.GetNames(typeof(AnswerTypeEnum)).Length;
            if (Mode == ModeEnum.Spelling)
            {
                multiplier = 1;
            }
            return multiplier;
        }

        private bool CanCurrentQuestionBeTheLastOne()
        {
            int numOfEntryRequiredAnswers = GetEntryNumberOfQuestions(QuestionTypeEnum.Closed)
                + GetEntryNumberOfQuestions(QuestionTypeEnum.Open);
            return GetNumberOfQuestions() - GetSumResult() <= numOfEntryRequiredAnswers;
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
            return currentValue >= GetNumOfRequiredAnswers(questionTypeEnum);
        }

        public bool IsQuestionRepeated(QuestionInfo nextQuestionInfo)
        {
            return nextQuestionInfo.Entry.Equals(CurrentQuestionInfo?.Entry);
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
                    numOfRequiredAnswers = Settings.NumOfOpenQuestions;
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
            List<string> correctAnswers = CurrentQuestionInfo.GetCorrectAnswers();
            correctAnswers.Sort();

            bool result = true;
            bool answersAreCorrect = string.Join(',', answerWords).Equals(
                string.Join(',', correctAnswers),
                StringComparison.InvariantCultureIgnoreCase
            );
            if (answersAreCorrect)
            {
                UpdateAnswersRegister(1);
            }
            else
            {
                result = false;
                UpdateAnswersRegister(0, UpdateAnswersRegisterOperationTypeEnum.Set, AnswerTypeEnum.Translations);
                UpdateAnswersRegister(0, UpdateAnswersRegisterOperationTypeEnum.Set, AnswerTypeEnum.Words);
            }

            return new AnswerResult(result, answerWords, correctAnswers);
        }

        public void UpdateAnswersRegister(
            int value,
            UpdateAnswersRegisterOperationTypeEnum operationType = UpdateAnswersRegisterOperationTypeEnum.Add,
            AnswerTypeEnum? answerType = null)
        {
            if (CurrentQuestionInfo == null)
            {
                return;
            }
            UpdateAnswersRegister(CurrentQuestionInfo, value, operationType, answerType);
        }

        private void UpdateAnswersRegister(
            QuestionInfo questionInfo,
            int value,
            UpdateAnswersRegisterOperationTypeEnum operationType = UpdateAnswersRegisterOperationTypeEnum.Add,
            AnswerTypeEnum? answerType = null)
        {
            string answerTypeKey = questionInfo.AnswerType.ToString("g");
            if (answerType != null)
            {
                answerTypeKey = answerType?.ToString("g") ?? "";
            }
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
