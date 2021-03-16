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
            LocationService locationService)
        {
            ConfigService = configService;
            Logger = logger;
            JsonService = jsonService;
            LocationService = locationService;
        }

        private ConfigService<AppSettings> ConfigService { get; set; }

        private ILogger<LearningModeExecutor> Logger { get; set; }

        private JsonService JsonService { get; set; }

        private LocationService LocationService { get; set; }

        private WordsSettings WordsSettings { get; set; } = new WordsSettings();

        private LearningSettings LearningSettings { get; set; } = new LearningSettings();

        private List<string> FilePaths { get; set; } = new List<string>();

        public async Task ExecuteAsync(List<string>? args = null)
        {
            VerifyParameters(args);
            Manager modeManager = GetModeManager();
            foreach (Question? question in modeManager.GetQuestions())
            {
                if (question == null)
                {
                    break;
                }
                // Show question.
                (int closedQuestionsResult, int openQuestionsResult) = modeManager.GetResult();
                PresentQuestion(
                    question, 
                    closedQuestionsResult, 
                    modeManager.GetNumberOfQuestions(QuestionTypeEnum.Closed), 
                    openQuestionsResult, 
                    modeManager.GetNumberOfQuestions(QuestionTypeEnum.Open)
                );
                // Verify answer.
                string answer = ReadAnswer();
                AnswerResult? answerResult = modeManager.VerifyAnswer(answer);
                bool isAnswerCorrect = answerResult?.Result ?? false;
                string correctAnswer = string.Join(", ", answerResult?.PossibleAnswers ?? new List<string>());
                // Show result.
                PresentResult(isAnswerCorrect, correctAnswer);
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
                    modeManager.UpdateAnswersRegister(2);
                }
                // Save logs in file.
                WriteLog(
                    isAnswerCorrect,
                    question.Content,
                    correctAnswer,
                    closedQuestionsResult + openQuestionsResult,
                    modeManager.GetNumberOfQuestions(),
                    modeManager.AnswersRegister
                );
                // Save history in database.
                //if (modeManager.CurrentEntry != null)
                //{
                //    await MaintainingHistoryService.SaveAsync(
                //        modeManager.CurrentEntry.SetPath.Namespace,
                //        modeManager.CurrentEntry.SetPath.Name,
                //        question.Content,
                //        answer,
                //        correctAnswer,
                //        isAnswerCorrect
                //    );
                //}
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
                throw new ArgsException("There are no arguments");
            }
            if (args.Count == 0)
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
            var modeManager = new Manager(setModeOperator, LearningSettings);
            return modeManager;
        }

        private void PresentQuestion(
            Question question, 
            int closedQuestionsCurrentResult, 
            int numberOfClosedQuestions, 
            int openQuestionsCurrentResult, 
            int numberOfOpenQuestions)
        {
            Console.Clear();
            Console.Write("\\p Play pronunciation; ");
            Console.Write("(Enter) Answer; ");
            Console.Write("\\o Override; ");
            Console.Write("\\r Restart; ");
            Console.Write("\\c Close;");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Closed questions result: {closedQuestionsCurrentResult}/{numberOfClosedQuestions}");
            Console.WriteLine($"Open questions result: {openQuestionsCurrentResult}/{numberOfOpenQuestions}");
            Console.WriteLine();
            Console.WriteLine($"  {question}");
            if (question.PossibleAnswers != null && question.PossibleAnswers.Count > 0)
            {
                for (int i = 1; i <= question.PossibleAnswers.Count; i++)
                {
                    Console.WriteLine($"  {i}. {question.PossibleAnswers[i]}");
                }
            }
            Console.WriteLine("  ------------------------------");
            Console.Write("  # ");
        }

        private string ReadAnswer()
        {
            string answer = Console.ReadLine() ?? "";
            return answer;
        }

        private void PresentResult(bool result, string? correctAnswer = null)
        {
            ConsoleColor standardForegroundColor = Console.ForegroundColor;
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
                case "\\p":
                    // play pronuciation
                    break;
            }
            return CommandEnum.None;
        }

        private void WriteLog(
            bool result,
            string question,
            string answers,
            int currentResult,
            int numberOfQuestions,
            Dictionary<QuestionTypeEnum, Dictionary<string, AnswersRegister>> answersRegister)
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
            Console.Write("The end :) ");
            Console.ForegroundColor = standardForegroundColor;
            Console.ReadLine();
        }
    }
}
