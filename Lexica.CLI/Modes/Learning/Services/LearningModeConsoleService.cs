using Lexica.CLI.Core.Services;
using Lexica.CLI.Modes.Learning.Models;
using Lexica.Learning.Models;
using System;

namespace Lexica.CLI.Modes.Learning.Services
{
    class LearningModeConsoleService : IService
    {
        public LearningModeConsoleService(BuildService buildService, VersionService versionService)
        {
            BuildService = buildService;
            VersionService = versionService;
        }

        public BuildService BuildService { get; private set; }

        public VersionService VersionService { get; private set; }

        public void SetVersionInWindowTitle()
        {
            string appName = "Lexica";
            string version = VersionService.GetVersion();
            string build = BuildService.GetBuild();
            Console.Title = $"{appName} {version} ({build})";
        }

        public void ClearConsole()
        {
            Console.Clear();
        }

        public void PresentQuestion(
            ModeEnum mode,
            Question question,
            ResultStatus closedQuestionsResultStatus,
            ResultStatus openQuestionsResultStatus,
            ResultStatus currentQuestionResultStatus,
            string answer = "",
            bool beforeVerification = true)
        {
            int lineAfterRendering = 0;
            switch (mode)
            {
                case ModeEnum.Spelling:
                    lineAfterRendering = 6;
                    break;
                case ModeEnum.OnlyOpen:
                    lineAfterRendering = 7;
                    break;
                case ModeEnum.Full:
                    lineAfterRendering = 8;
                    break;
            }
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
            switch (mode)
            {
                case ModeEnum.Full:
                    Console.Write($"  Closed questions result: ".PadRight(27));
                    Console.WriteLine(closedQuestionsResultStatus.ToString(leftPad: 4).PadRight(80));
                    Console.Write($"  Open questions result: ".PadRight(27));
                    Console.WriteLine(openQuestionsResultStatus.ToString(leftPad: 4).PadRight(80));
                    break;
                default:
                    Console.Write($"  Result: ".PadRight(27));
                    Console.WriteLine(openQuestionsResultStatus.ToString(leftPad: 4).PadRight(80));
                    break;
            }
            Console.Write($"  Current question result: ".PadRight(27));
            Console.WriteLine(currentQuestionResultStatus.ToString(leftPad: 4).PadRight(80));
            Console.WriteLine();
            if (mode != ModeEnum.Spelling)
            {
                var previousForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"  {question.Content}".PadRight(80));
                Console.ForegroundColor = previousForegroundColor;
            }
            if (question.PossibleAnswers != null && question.PossibleAnswers.Count > 0)
            {
                lineAfterRendering = 12;
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
            Console.WriteLine(" ".PadRight(80));
            Console.SetCursorPosition(4, lineAfterRendering);
        }

        public string ReadAnswer()
        {
            string answer = Console.ReadLine() ?? "";
            return answer;
        }

        public CommandEnum HandleCommand()
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
                default:
                    break;
            }
            return CommandEnum.None;
        }

        public void PresentResult(
            ModeEnum mode,
            Question question,
            ResultStatus closedQuestionsResultStatus,
            ResultStatus openQuestionsResultStatus,
            ResultStatus currentQuestionResultStatus,
            string answer,
            bool result,
            string correctAnswer,
            string translationsInfo)
        {
            ConsoleColor standardForegroundColor = Console.ForegroundColor;
            PresentQuestion(
                mode,
                question,
                closedQuestionsResultStatus,
                openQuestionsResultStatus,
                currentQuestionResultStatus,
                answer,
                false
            );
            Console.WriteLine();
            if (result)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine();
                Console.Write("  Correct answer");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.Write("  Wrong answer");
                if (correctAnswer != null)
                {
                    Console.WriteLine();
                    Console.Write("  Correct answer is: ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(correctAnswer);
                }
            }
            if (mode == ModeEnum.Spelling)
            {
                Console.WriteLine();
                Console.ForegroundColor = standardForegroundColor;
                Console.Write($"  Translations: ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(translationsInfo);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("  ");
            Console.ForegroundColor = standardForegroundColor;
        }

        public void ShowSummary()
        {
            ClearConsole();
            ConsoleColor standardForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(" The end");
            Console.ForegroundColor = standardForegroundColor;
        }
    }
}
