using Lexica.CLI.Args;
using Lexica.CLI.Core.Config;
using Lexica.CLI.Core.Services;
using Lexica.CLI.Executors;
using Lexica.CLI.Modes.Learning.Models;
using Lexica.Core.IO;
using Lexica.Core.Models;
using Lexica.Core.Services;
using Lexica.LearningMode;
using Lexica.LearningMode.Config;
using Lexica.LearningMode.Models;
using Lexica.LearningMode.Services;
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

namespace Lexica.CLI.Modes.Learning
{
    class LearningModeExecutor : IAsyncExecutor
    {
        public LearningModeExecutor(
            ConfigService<AppSettings> configService,
            ILogger<LearningModeExecutor> logger,
            JsonService jsonService,
            LocationService locationService,
            ILearningHistoryService learningHistoryService,
            IPronunciation pronunciationService,
            ILogger<IPronunciation> pronunciationLogger)
        {
            ConfigService = configService;
            Logger = logger;
            JsonService = jsonService;
            LocationService = locationService;
            LearningHistoryService = learningHistoryService;
            PronunciationService = pronunciationService;
            PronunciationLogger = pronunciationLogger;
        }

        private ConfigService<AppSettings> ConfigService { get; set; }

        private ILogger<LearningModeExecutor> Logger { get; set; }

        private JsonService JsonService { get; set; }

        private LocationService LocationService { get; set; }

        public ILearningHistoryService LearningHistoryService { get; set; }

        private IPronunciation PronunciationService { get; set; }

        private ILogger<IPronunciation> PronunciationLogger { get; set; }

        private WordsSettings WordsSettings { get; set; } = new WordsSettings();

        private LearningSettings LearningSettings { get; set; } = new LearningSettings();

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
                if (LearningSettings.PlayPronuncation.TranslationsMode 
                    && modeManager.CurrentQuestionInfo?.ModeType == ModeTypeEnum.Words)
                {
                    PlayPronunciation(modeManager.CurrentQuestionInfo.Entry.Words ?? new List<string>());
                }
                // Show question.
                PresentQuestion(
                    question,
                    modeManager.GetResult(QuestionTypeEnum.Closed), 
                    modeManager.GetNumberOfQuestions(QuestionTypeEnum.Closed), 
                    modeManager.GetResult(QuestionTypeEnum.Open), 
                    modeManager.GetNumberOfQuestions(QuestionTypeEnum.Open)
                );
                // Verify answer.
                string answer = ReadAnswer();
                AnswerResult? answerResult = modeManager.VerifyAnswer(answer);
                bool isAnswerCorrect = answerResult?.Result ?? false;
                string correctAnswer = string.Join(", ", answerResult?.PossibleAnswers ?? new List<string>());
                // Show result.
                PresentResult(
                    question,
                    modeManager.GetResult(QuestionTypeEnum.Closed),
                    modeManager.GetNumberOfQuestions(QuestionTypeEnum.Closed),
                    modeManager.GetResult(QuestionTypeEnum.Open),
                    modeManager.GetNumberOfQuestions(QuestionTypeEnum.Open),
                    answer, 
                    isAnswerCorrect, 
                    correctAnswer
                );
                // Play pronunciation.
                if (LearningSettings.PlayPronuncation.WordsMode
                    && modeManager.CurrentQuestionInfo?.ModeType == ModeTypeEnum.Translations)
                {
                    PlayPronunciation(modeManager.CurrentQuestionInfo.Entry.Words ?? new List<string>());
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
                    modeManager.GetResult(QuestionTypeEnum.Closed),
                    modeManager.GetNumberOfQuestions(QuestionTypeEnum.Closed),
                    modeManager.GetResult(QuestionTypeEnum.Open),
                    modeManager.GetNumberOfQuestions(QuestionTypeEnum.Open),
                    modeManager.AnswersRegister
                );
                // Save history in database.
                if (modeManager.CurrentQuestionInfo != null)
                {
                    await LearningHistoryService.SaveAsync(
                        modeManager.CurrentQuestionInfo.Entry.SetPath.Namespace,
                        modeManager.CurrentQuestionInfo.Entry.SetPath.Name,
                        question.Content,
                        modeManager.CurrentQuestionInfo.QuestionType.ToString().ToLower(),
                        answer,
                        correctAnswer,
                        isAnswerCorrect
                    );
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
            if (ConfigService.Config?.Learning == null)
            {
                throw new Exception("Learning settings are empty.");
            }
            LearningSettings = ConfigService.Config.Learning;
            if (args == null || args.Count == 0)
            {
                throw new ArgsException("There are no file paths arguments.");
            }
            FilePaths = new List<string>();
            for (int i = 0; i < args.Count; i++)
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
            var modeManager = new Manager(setModeOperator, LearningSettings);
            return modeManager;
        }

        private void PlayPronunciation(List<string> words)
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
            Question question, 
            int closedQuestionsCurrentResult, 
            int numberOfClosedQuestions, 
            int openQuestionsCurrentResult, 
            int numberOfOpenQuestions,
            string answer = "",
            bool beforeVerification = true)
        {
            int lineAfterRendering = 7;
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
            Console.WriteLine($"  Closed questions result: {closedQuestionsCurrentResult}/{numberOfClosedQuestions}".PadRight(80));
            Console.WriteLine($"  Open questions result: {openQuestionsCurrentResult}/{numberOfOpenQuestions}".PadRight(80));
            Console.WriteLine();
            var previousForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  {question.Content}".PadRight(80));
            Console.ForegroundColor = previousForegroundColor;
            if (question.PossibleAnswers != null && question.PossibleAnswers.Count > 0)
            {
                lineAfterRendering = 11;
                for (int i = 0; i < question.PossibleAnswers.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {question.PossibleAnswers[i]}".PadRight(80));
                }
            }
            Console.WriteLine("  ".PadRight(80, '-'));
            Console.Write("  # ".PadRight(80));
            if (answer.Length > 0)
            {
                Console.SetCursorPosition(4, lineAfterRendering);
                Console.Write(answer);
            }
            Console.WriteLine();
            Console.WriteLine(" ".PadRight(80));
            Console.WriteLine(" ".PadRight(80));
            Console.WriteLine(" ".PadRight(80));
            Console.WriteLine(" ".PadRight(80));
            Console.WriteLine(" ".PadRight(80));
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
            Question question,
            int closedQuestionsCurrentResult,
            int numberOfClosedQuestions,
            int openQuestionsCurrentResult,
            int numberOfOpenQuestions,
            string answer,
            bool result,
            string? correctAnswer = null)
        {
            ConsoleColor standardForegroundColor = Console.ForegroundColor;
            PresentQuestion(
                question,
                closedQuestionsCurrentResult,
                numberOfClosedQuestions,
                openQuestionsCurrentResult,
                numberOfOpenQuestions,
                answer,
                false
            );
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
                if (correctAnswer != null)
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
            int closedQuestionsCurrentResult,
            int numberOfClosedQuestions,
            int openQuestionsCurrentResult,
            int numberOfOpenQuestions,
            Dictionary<QuestionTypeEnum, Dictionary<string, AnswerRegister>> answersRegister)
        {
            JsonSerializerOptions options = JsonService.GetJsonSerializerOptions(true, null);
            string logData = JsonSerializer.Serialize(
                new
                {
                    Result = result,
                    Question = question,
                    Answers = answers,
                    ClosedQuestionsProgress = $"{closedQuestionsCurrentResult}/{numberOfClosedQuestions}",
                    OpenQuestionsProgress = $"{openQuestionsCurrentResult}/{numberOfOpenQuestions}",
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
