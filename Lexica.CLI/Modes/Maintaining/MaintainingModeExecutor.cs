using Lexica.CLI.Args;
using Lexica.CLI.Core.Config;
using Lexica.CLI.Core.Services;
using Lexica.CLI.Executors;
using Lexica.CLI.Modes.Maintaining.Models;
using Lexica.Core.IO;
using Lexica.Core.Models;
using Lexica.Core.Services;
using Lexica.MaintainingMode;
using Lexica.MaintainingMode.Config;
using Lexica.MaintainingMode.Models;
using Lexica.MaintainingMode.Services;
using Lexica.Pronunciation;
using Lexica.Words;
using Lexica.Words.Config;
using Lexica.Words.Services;
using Lexica.Words.Validators;
using Lexica.Words.Validators.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lexica.CLI.Modes.Maintaining
{
    class MaintainingModeExecutor : IAsyncExecutor
    {
        public MaintainingModeExecutor(
            ConfigService<AppSettings> configService,
            ILogger<MaintainingModeExecutor> logger,
            JsonService jsonService,
            LocationService locationService,
            IMaintainingHistoryService maintainingHistoryService,
            IPronunciation pronunciationService,
            ILogger<IPronunciation> pronunciationLogger)
        {
            ConfigService = configService;
            Logger = logger;
            JsonService = jsonService;
            LocationService = locationService;
            MaintainingHistoryService = maintainingHistoryService;
            PronunciationService = pronunciationService;
            PronunciationLogger = pronunciationLogger;
        }

        private ConfigService<AppSettings> ConfigService { get; set; }

        private ILogger<MaintainingModeExecutor> Logger { get; set; }

        private JsonService JsonService { get; set; }

        private LocationService LocationService { get; set; }

        private IMaintainingHistoryService MaintainingHistoryService { get; set; }

        private IPronunciation PronunciationService { get; set; }

        private ILogger<IPronunciation> PronunciationLogger { get; set; }

        private WordsSettings WordsSettings { get; set; } = new WordsSettings();

        private MaintainingSettings MaintainingSettings { get; set; } = new MaintainingSettings();

        private ModeTypeEnum ModeType { get; set; } = ModeTypeEnum.Translations;

        private List<string> FilePaths { get; set; } = new List<string>();

        public async Task ExecuteAsync(List<string>? args = null)
        {
            Console.Clear();
            VerifyParameters(args);
            Manager modeManager = GetModeManager();
            foreach (Question? question in modeManager.GetQuestions())
            {
                if (question == null)
                {
                    break;
                }
                List<string> words = modeManager.CurrentEntry?.Words ?? new List<string>();
                // Play pronunciation.
                if (ModeType == ModeTypeEnum.Pronunciation)
                {
                    if (!await PlayPronunciation(words))
                    {
                        modeManager.UpdateAnswersRegister(2);
                        continue;
                    }
                }
                // Play background pronunciation.
                if (ModeType == ModeTypeEnum.Words && MaintainingSettings.PlayPronuncation.WordsMode)
                {
                    PlayBackgroundPronunciation(words);
                }
                // Show question.
                string questionContent = "";
                if (ModeType != ModeTypeEnum.Pronunciation)
                {
                    questionContent = question.Content;
                }
                PresentQuestion(questionContent, modeManager.GetResult(), modeManager.GetNumberOfQuestions());
                // Verify answer.
                string answer = ReadAnswer();
                AnswerResult? answerResult = modeManager.VerifyAnswer(answer);
                bool isAnswerCorrect = answerResult?.Result ?? false;
                string correctAnswer = string.Join(", ", answerResult?.PossibleAnswers ?? new List<string>());
                // Show result.
                PresentResult(
                    questionContent, 
                    modeManager.GetResult(), 
                    modeManager.GetNumberOfQuestions(),
                    answer,
                    isAnswerCorrect, 
                    correctAnswer
                );
                // Play background pronunciation.
                if (ModeType == ModeTypeEnum.Translations && MaintainingSettings.PlayPronuncation.TranslationsMode)
                {
                    PlayBackgroundPronunciation(words);
                }
                // Handle user commands (shortcuts).
                CommandEnum command = HandleCommand();
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
                    modeManager.OverridePreviousMistake();
                }
                // Save logs in file.
                WriteLog(
                    isAnswerCorrect,
                    question.Content,
                    correctAnswer,
                    modeManager.GetResult(),
                    modeManager.GetNumberOfQuestions(),
                    modeManager.AnswersRegister
                );
                // Save history in database.
                if (modeManager.CurrentEntry != null)
                {
                    await MaintainingHistoryService.SaveAsync(
                        modeManager.CurrentEntry.SetPath.Namespace,
                        modeManager.CurrentEntry.SetPath.Name,
                        question.Content,
                        answer,
                        correctAnswer,
                        isAnswerCorrect
                    );
                }
                // Reset after wrong answer.
                if (command != CommandEnum.Override && MaintainingSettings.ResetAfterMistake && !isAnswerCorrect)
                {
                    await ExecuteAsync(args);
                    return;
                }
            }
            ShowSummary();
        }

        private void VerifyParameters(List<string>? args = null)
        {
            if (ConfigService.Config?.Words == null)
            {
                throw new Exception("Words settings are empty.");
            }
            WordsSettings = ConfigService.Config.Words;
            if (ConfigService.Config?.Maintaining == null)
            {
                throw new Exception("Maintaining settings are empty.");
            }
            MaintainingSettings = ConfigService.Config.Maintaining;
            if (args == null || args.Count == 0)
            {
                throw new ArgsException("There are no arguments");
            }
            bool enumParsingResult = Enum.TryParse(args[0], out ModeTypeEnum mode);
            if (!enumParsingResult)
            {
                throw new ArgsException("Mode type argument is incorrect.");
            }
            ModeType = mode;
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

        private Manager GetModeManager()
        {
            var fileValidator = new FileValidator(new ValidationData());
            var setService = new SetService(fileValidator);
            var fileSources = new List<ISource>();
            for (int i = 0; i < FilePaths.Count; i++)
            {
                string absolutePath = WordsSettings.DirectoryPath ?? LocationService.GetExecutingAssemblyLocation();
                if (FilePaths[i].StartsWith('.'))
                {
                    absolutePath = LocationService.GetExecutingAssemblyLocation();
                }
                string filePath = Path.Combine(absolutePath, FilePaths[i].TrimStart('.', '\\', '/'));
                fileSources.Add(new FileSource(filePath));
            }
            var setModeOperator = new SetModeOperator(setService, fileSources);
            var modeManager = new Manager(setModeOperator, ModeType);
            return modeManager;
        }

        private async Task<bool> PlayPronunciation(List<string> words)
        {
            return await PronunciationService.PlayAsync(words);
        }

        private void PlayBackgroundPronunciation(List<string> words)
        {
            _ = PronunciationService.PlayAsync(words)
                .ContinueWith(
                    x => PronunciationLogger.LogError(
                        x.Exception, "An unexpected error occured in pronunciation service."
                    ),
                    TaskContinuationOptions.OnlyOnFaulted
                );
        }

        private void PresentQuestion(
            string question, 
            int currentResult, 
            int numberOfQuestions,
            string answer = "", 
            bool beforeVerification = true)
        {
            int lineAfterRendering = 5;
            Console.SetCursorPosition(0, 0);
            if (beforeVerification)
            {
                Console.Write(" (Enter) Answer".PadRight(80));
            }
            else
            {
                Console.Write(" (Enter) Next question;");
                Console.Write(" \\o Override;");
                Console.Write(" \\r Restart;");
                Console.Write(" \\c Close;");
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"  Result: {currentResult}/{numberOfQuestions}".PadRight(80));
            Console.WriteLine();
            if (question.Length > 0)
            {
                lineAfterRendering = 6;
                var previousForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  {question}".PadRight(80));
                Console.ForegroundColor = previousForegroundColor;
            }
            Console.WriteLine("  ".PadRight(80, '-'));
            Console.Write("  # ".PadRight(80));
            if (answer.Length > 0)
            {
                Console.SetCursorPosition(4, lineAfterRendering);
                Console.Write(answer);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(" ".PadRight(80));
            Console.WriteLine(" ".PadRight(80));
            Console.SetCursorPosition(4, lineAfterRendering);
        }

        private string ReadAnswer()
        {
            string answer = Console.ReadLine() ?? "";
            return answer;
        }

        private void PresentResult(
            string question,
            int currentResult,
            int numberOfQuestions,
            string answer,
            bool result, 
            string correctAnswer)
        {
            ConsoleColor standardForegroundColor = Console.ForegroundColor;
            PresentQuestion(question, currentResult, numberOfQuestions, answer, false);
            Console.WriteLine();
            if (result)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.Write("  Correct answer :)  ");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.Write("  Wrong answer :(  ");
                if (correctAnswer.Length > 0)
                {
                    Console.WriteLine();
                    Console.Write("  Correct answer is: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{correctAnswer}  ");
                }
            }
            Console.ForegroundColor = standardForegroundColor;
        }

        private CommandEnum HandleCommand()
        {
            string? input = Console.ReadLine();
            switch (input)
            {
                case "\\o":
                    return CommandEnum.Override;
                case "\\r":
                    return CommandEnum.Restart;
                case "\\c":
                    return CommandEnum.Close;
            }
            return CommandEnum.None;
        }

        private void WriteLog(
            bool result,
            string question,
            string answers,
            int currentResult,
            int numberOfQuestions,
            Dictionary<string, AnswerRegister> answersRegister)
        {
            JsonSerializerOptions options = JsonService.GetJsonSerializerOptions(true, null);
            string logData = JsonSerializer.Serialize(
                new
                {
                    Result = result,
                    Question = question,
                    Answers = answers,
                    Progress = $"{currentResult}/{numberOfQuestions}",
                    AnswersRegister = answersRegister
                },
                options
            );
            Logger.LogDebug(logData);
        }

        private void ShowSummary()
        {
            Console.Clear();
            ConsoleColor standardForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" The end :) ");
            Console.ForegroundColor = standardForegroundColor;
            Console.ReadLine();
        }
    }
}
