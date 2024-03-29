﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Lexica.CLI.Args;
using Lexica.CLI.Core.Services;
using Lexica.CLI.Executors;
using Lexica.CLI.Modes.Learning.Models;
using Lexica.CLI.Modes.Learning.Services;
using Lexica.Core.Extensions;
using Lexica.Core.IO;
using Lexica.Learning;
using Lexica.Learning.Config;
using Lexica.Learning.Models;
using Lexica.Learning.Services;
using Lexica.Pronunciation;
using Lexica.Pronunciation.Config;
using Lexica.Words;
using Lexica.Words.Config;
using Lexica.Words.Services;
using Lexica.Words.Validators;
using Lexica.Words.Validators.Models;
using Microsoft.Extensions.Logging;

namespace Lexica.CLI.Modes.Learning
{
    internal class LearningModeExecutor : IAsyncExecutor
    {
        public LearningModeExecutor(
            WordsSettings wordsSettings,
            LearningSettings learningSettings,
            PronunciationApiSettings pronunciationApiSettings,
            ILogger<LearningModeExecutor> logger,
            JsonService jsonService,
            ILearningHistoryService learningHistoryService,
            IPronunciation pronunciationService,
            ILogger<IPronunciation> pronunciationLogger,
            LearningModeConsoleService consoleService)
        {
            WordsSettings = wordsSettings;
            LearningSettings = learningSettings;
            PronunciationApiSettings = pronunciationApiSettings;
            Logger = logger;
            JsonService = jsonService;
            LearningHistoryService = learningHistoryService;
            PronunciationService = pronunciationService;
            PronunciationLogger = pronunciationLogger;
            ConsoleService = consoleService;
        }

        public WordsSettings WordsSettings { get; }

        public LearningSettings LearningSettings { get; }

        public PronunciationApiSettings PronunciationApiSettings { get; }

        private ILogger<LearningModeExecutor> Logger { get; }

        private JsonService JsonService { get; }

        public ILearningHistoryService LearningHistoryService { get; set; }

        private IPronunciation PronunciationService { get; }

        private ILogger<IPronunciation> PronunciationLogger { get; }

        private LearningModeConsoleService ConsoleService { get; }

        private ModeEnum Mode { get; set; }

        private List<string> FilePaths { get; set; } = new List<string>();

        private enum WhenEnum
        {
            BeforeAnswer = 0,
            AfterAnswer = 1
        };

        public async Task ExecuteAsync(List<string>? args = null)
        {
            VerifyParameters(args);
            ConsoleService.SetVersionInWindowTitle();
            ConsoleService.ClearConsole();
            LearningModeOperator modeOperator = GetLearningModeOperator();
            IEnumerable<Question?> questionsEnumerable = modeOperator.GetQuestions(
                randomizeEachIteration: true,
                pieceSize: Mode == ModeEnum.Full ? GetPieceSize() : -1
            );
            foreach (Question? question in questionsEnumerable)
            {
                if (question == null || modeOperator.CurrentQuestionInfo == null)
                {
                    break;
                }
                if (Mode == ModeEnum.Spelling
                    && !await PronunciationAudioExists(modeOperator.CurrentQuestionInfo.Entry.Words))
                {
                    modeOperator.UpdateAnswersRegister(LearningSettings.NumOfOpenQuestions);
                    continue;
                }
                PlayPronunciationAudio(
                    modeOperator.CurrentQuestionInfo.Entry.Words,
                    WhenEnum.BeforeAnswer,
                    modeOperator.CurrentQuestionInfo.AnswerType
                );
                ConsoleService.PresentQuestion(
                    Mode,
                    question,
                    new ResultStatus(
                        modeOperator.GetResult(QuestionTypeEnum.Closed),
                        modeOperator.GetNumberOfQuestions(QuestionTypeEnum.Closed)
                    ),
                    new ResultStatus(
                        modeOperator.GetResult(QuestionTypeEnum.Open),
                        modeOperator.GetNumberOfQuestions(QuestionTypeEnum.Open)
                    ),
                    new ResultStatus(
                        modeOperator.GetCurrentQuestionResult(),
                        modeOperator.GetNumberOfCurrentQuestions()
                    )
                );
                string answer = ConsoleService.ReadAnswer();
                AnswerResult? answerResult = modeOperator.VerifyAnswer(answer);
                bool isAnswerCorrect = answerResult?.Result ?? false;
                string givenAnswer = answerResult?.GivenAnswers == null
                    ? answer
                    : string.Join(", ", answerResult.GivenAnswers);
                string correctAnswer = string.Join(", ", answerResult?.CorrectAnswers ?? new List<string>());
                ConsoleService.PresentResult(
                    Mode,
                    question,
                    new ResultStatus(
                        modeOperator.GetResult(QuestionTypeEnum.Closed),
                        modeOperator.GetNumberOfQuestions(QuestionTypeEnum.Closed)
                    ),
                    new ResultStatus(
                        modeOperator.GetResult(QuestionTypeEnum.Open),
                        modeOperator.GetNumberOfQuestions(QuestionTypeEnum.Open)
                    ),
                    new ResultStatus(
                        modeOperator.GetCurrentQuestionResult(),
                        modeOperator.GetNumberOfCurrentQuestions()
                    ),
                    answer,
                    isAnswerCorrect,
                    correctAnswer,
                    modeOperator.CurrentQuestionInfo.Entry.ToString(Words.Models.EntryPartEnum.Translations)
                );
                PlayPronunciationAudio(
                    modeOperator.CurrentQuestionInfo.Entry.Words,
                    WhenEnum.AfterAnswer,
                    modeOperator.CurrentQuestionInfo.AnswerType
                );
                CommandEnum command = ConsoleService.HandleCommand();
                if (command == CommandEnum.Close)
                {
                    break;
                }
                else if (command == CommandEnum.Restart)
                {
                    await ExecuteAsync(args);
                    return;
                }
                else if (command == CommandEnum.Override && !isAnswerCorrect)
                {
                    modeOperator.OverridePreviousMistake();
                }
                await LearningHistoryService.SaveRecordToDbAsync(
                    modeOperator.CurrentQuestionInfo.Entry.SetPath.Namespace,
                    modeOperator.CurrentQuestionInfo.Entry.SetPath.Name,
                    Mode.ToString().ToLower(),
                    question.Content,
                    modeOperator.CurrentQuestionInfo.QuestionType.ToString().ToLower(),
                    givenAnswer,
                    correctAnswer,
                    isAnswerCorrect
                );
                WriteLog(
                    isAnswerCorrect,
                    question.Content,
                    correctAnswer,
                    new ResultStatus(
                        modeOperator.GetResult(QuestionTypeEnum.Closed),
                        modeOperator.GetNumberOfQuestions(QuestionTypeEnum.Closed)
                    ),
                    new ResultStatus(
                        modeOperator.GetResult(QuestionTypeEnum.Open),
                        modeOperator.GetNumberOfQuestions(QuestionTypeEnum.Open)
                    ),
                    modeOperator.AnswersRegister
                );
                if (ShouldResetMode(isAnswerCorrect, command))
                {
                    await ExecuteAsync(args);
                    return;
                }
            }

            ConsoleService.ShowSummary();
        }

        private void VerifyParameters(List<string>? args = null)
        {
            if (args == null || args.Count == 0)
            {
                throw new ArgsException("There are no arguments.");
            }
            string modeArg = string.Concat(args[0].ToLower().Split('-').Select(x => x.UppercaseFirst()));
            bool modeEnumParsingResult = Enum.TryParse(modeArg, out ModeEnum mode);
            if (!modeEnumParsingResult)
            {
                throw new ArgsException("Mode type argument is incorrect.");
            }
            Mode = mode;
            if (args.Count == 1)
            {
                throw new ArgsException("There are no file paths arguments.");
            }
            FilePaths = new List<string>();
            for (int i = 1; i < args.Count; i++)
            {
                FilePaths.Add(args[i]);
            }
        }

        private LearningModeOperator GetLearningModeOperator()
        {
            var fileValidator = new FileValidator(new ValidationData());
            var setService = new SetService(fileValidator);
            var fileSources = new List<ISource>();
            for (int i = 0; i < FilePaths.Count; i++)
            {
                string absolutePath = WordsSettings.SetsDirectoryPath ?? AppDomain.CurrentDomain.BaseDirectory;
                if (FilePaths[i].StartsWith('.'))
                {
                    absolutePath = AppDomain.CurrentDomain.BaseDirectory;
                }
                string filePath = Path.Combine(absolutePath, FilePaths[i].TrimStart('.', '\\', '/'));
                SetFileExists(filePath);
                fileSources.Add(new FileSource(filePath));
            }
            var wordsSetOperator = new WordsSetOperator(setService, fileSources);
            var learningModeOperator = new LearningModeOperator(wordsSetOperator, LearningSettings, Mode);
            return learningModeOperator;
        }

        private void SetFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgsException($"Set file {filePath} doesn't exist.");
            }
        }

        private int GetPieceSize()
        {
            int pieceSize = -1;
            if (Mode == ModeEnum.Full)
            {
                pieceSize = 7;
            }
            return pieceSize;
        }

        private async Task<bool> PronunciationAudioExists(List<string> words)
        {
            return await PronunciationService.AudioExists(words);
        }

        private void PlayPronunciationAudio(List<string> words, WhenEnum whenWasInvoked, AnswerTypeEnum answerType)
        {
            if (whenWasInvoked == WhenEnum.BeforeAnswer && !AudioCanBePlayedBeforeAnswer(answerType))
            {
                return;
            }
            if (whenWasInvoked == WhenEnum.AfterAnswer && !AudioCanBePlayedAfterAnswer(answerType))
            {
                return;
            }
            _ = PronunciationService.PlayAsync(words.ToList())
                .ContinueWith(
                    x => PronunciationLogger.LogError(
                        x.Exception, "An unexpected error occured in pronunciation service."
                    ),
                    TaskContinuationOptions.OnlyOnFaulted
                );
        }

        private bool AudioCanBePlayedBeforeAnswer(AnswerTypeEnum answerType)
        {
            return Mode == ModeEnum.Spelling
                || (LearningSettings.PlayPronuncation.BeforeAnswer && answerType == AnswerTypeEnum.Translations);
        }

        private bool AudioCanBePlayedAfterAnswer(AnswerTypeEnum answerType)
        {
            return Mode != ModeEnum.Spelling
                && LearningSettings.PlayPronuncation.AfterAnswer
                && answerType == AnswerTypeEnum.Words;
        }

        private void WriteLog(
            bool result,
            string question,
            string answers,
            ResultStatus closedQuestionsResultStatus,
            ResultStatus openQuestionsResultStatus,
            Dictionary<QuestionTypeEnum, Dictionary<string, AnswerRegister>> answersRegister)
        {
            if (LearningSettings.SaveDebugLogs)
            {
                JsonSerializerOptions options = JsonService.GetJsonSerializerOptions(true, null);
                string logData = JsonSerializer.Serialize(
                    new
                    {
                        Result = result,
                        Question = question,
                        Answers = answers,
                        ClosedQuestionsProgress = closedQuestionsResultStatus.ToString(),
                        OpenQuestionsProgress = openQuestionsResultStatus.ToString(),
                        AnswersRegister = answersRegister
                    },
                    options
                );
                Logger.LogDebug(logData);
            }
        }

        private bool ShouldResetMode(bool isAnswerCorrect, CommandEnum command)
        {
            return LearningSettings.ResetAfterMistake && !isAnswerCorrect && command != CommandEnum.Override;
        }
    }
}
