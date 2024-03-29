﻿using System;
using System.Collections.Generic;
using System.Text;
using Lexica.CLI.Core.Services;
using Lexica.CLI.Modes.Learning.Models;
using Lexica.Core.Extensions;
using Lexica.Learning.Models;

namespace Lexica.CLI.Modes.Learning.Services
{
    internal class LearningModeConsoleService : IService
    {
        public LearningModeConsoleService(BuildService buildService, VersionService versionService)
        {
            BuildService = buildService;
            VersionService = versionService;
        }

        public BuildService BuildService { get; }

        public VersionService VersionService { get; }

        public void SetVersionInWindowTitle()
        {
            string appName = "Lexica";
            string version = VersionService.GetVersion();
            string build = BuildService.GetBuild();
            string title = $"{appName} {version}";
            if (build.Length > 0)
            {
                title += $" ({build})";
            }
            Console.Title = title;
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
                    lineAfterRendering = 6;
                    break;

                case ModeEnum.Full:
                    lineAfterRendering = 7;
                    break;
            }
            Console.SetCursorPosition(0, 0);
            if (beforeVerification)
            {
                Console.Write(" (Enter) Answer".PadRight(80));
            }
            else
            {
                Console.Write(" (Enter) Next question; o Override; r Restart; c Close;".PadRight(80));
            }
            Console.WriteLine();
            Console.WriteLine();
            switch (mode)
            {
                case ModeEnum.Full:
                    Console.Write("  Closed questions result: ".PadRight(27));
                    Console.WriteLine(closedQuestionsResultStatus.ToString(leftPad: 4).PadRight(80 - 27));
                    Console.Write("  Open questions result: ".PadRight(27));
                    Console.WriteLine(openQuestionsResultStatus.ToString(leftPad: 4).PadRight(80 - 27));
                    break;

                default:
                    Console.Write("  Result: ".PadRight(27));
                    Console.WriteLine(openQuestionsResultStatus.ToString(leftPad: 4).PadRight(80 - 27));
                    break;
            }
            Console.Write("  Current question result: ".PadRight(27));
            Console.WriteLine(currentQuestionResultStatus.ToString(leftPad: 4).PadRight(80 - 27));
            Console.WriteLine();
            if (mode != ModeEnum.Spelling)
            {
                var previousForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Blue;
                lineAfterRendering += ShowMultilineText(
                    question.Content,
                    firstLineLeftIndendation: 2,
                    nextLinesLeftIndendation: 2,
                    maxNumOfChars: 78
                );
                Console.ForegroundColor = previousForegroundColor;
            }
            if (question.PossibleAnswers?.Count > 0)
            {
                for (int i = 0; i < question.PossibleAnswers.Count; i++)
                {
                    Console.Write($"  {i + 1}. ");
                    lineAfterRendering += ShowMultilineText(
                        question.PossibleAnswers[i],
                        firstLineLeftIndendation: 0,
                        nextLinesLeftIndendation: 5,
                        maxNumOfChars: 75
                    );
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
            for (int i = 0; i < 12; i++)
            {
                Console.WriteLine(" ".PadRight(80));
            }
            Console.SetCursorPosition(4, lineAfterRendering);
        }

        public string ReadAnswer()
        {
            var sb = new StringBuilder();
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = ReadCharacter(sb);
                if (sb.Length == 76)
                {
                    break;
                }
            } while (CanGetNextChar(keyInfo));

            return sb.ToString();
        }

        private ConsoleKeyInfo ReadCharacter(StringBuilder stringBuilder)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (stringBuilder.Length > 0)
                {
                    Console.Write("\b \b");
                    stringBuilder.Length--;
                }
                return keyInfo;
            }
            if (IsKeyHandled(keyInfo))
            {
                Console.Write(keyInfo.KeyChar);
                stringBuilder.Append(keyInfo.KeyChar);
            }

            return keyInfo;
        }

        private bool IsKeyAChar(ConsoleKeyInfo keyInfo)
        {
            return keyInfo.Key >= ConsoleKey.A && keyInfo.Key <= ConsoleKey.Z && keyInfo.KeyChar != '\0';
        }

        private bool IsKeyADigit(ConsoleKeyInfo keyInfo)
        {
            bool isDigit = keyInfo.Key >= ConsoleKey.D0 && keyInfo.Key <= ConsoleKey.D9;
            bool isNumPadDigit = keyInfo.Key >= ConsoleKey.NumPad0 && keyInfo.Key <= ConsoleKey.NumPad9;
            return isDigit || isNumPadDigit;
        }

        private bool IsKeyHandled(ConsoleKeyInfo keyInfo)
        {
            if (IsKeyAChar(keyInfo))
            {
                return true;
            }
            if (IsKeyADigit(keyInfo))
            {
                return true;
            }
            var additionalChars = new List<char> { '.', ',', '-', ';', '\'', ' ' };
            return additionalChars.Contains(keyInfo.KeyChar);
        }

        private bool CanGetNextChar(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.KeyChar == -1)
            {
                return false;
            }
            if (keyInfo.KeyChar == '\n' || keyInfo.KeyChar == '\r')
            {
                return false;
            }
            return true;
        }

        public CommandEnum HandleCommand()
        {
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.KeyChar)
                {
                    case 'o':
                        return CommandEnum.Override;

                    case 'r':
                        return CommandEnum.Restart;

                    case 'c':
                        return CommandEnum.Close;

                    case '\n':
                    case '\r':
                        return CommandEnum.None;
                }
            }
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
                Console.WriteLine("  Correct answer");
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
                    ShowMultilineText(
                        correctAnswer,
                        firstLineLeftIndendation: 0,
                        nextLinesLeftIndendation: 21,
                        maxNumOfChars: 59
                    );
                }
            }
            if (mode == ModeEnum.Spelling)
            {
                Console.WriteLine();
                Console.ForegroundColor = standardForegroundColor;
                Console.Write("  Translations: ");
                Console.ForegroundColor = ConsoleColor.Blue;
                ShowMultilineText(
                    translationsInfo,
                    firstLineLeftIndendation: 0,
                    nextLinesLeftIndendation: 16,
                    maxNumOfChars: 64
                );
            }
            Console.WriteLine();
            Console.Write("  ");
            Console.ForegroundColor = standardForegroundColor;
        }

        private int ShowMultilineText(
            string txt, int firstLineLeftIndendation, int nextLinesLeftIndendation, int maxNumOfChars)
        {
            List<string> lines = txt.Split(maxNumOfChars);
            Console.Write("".PadRight(firstLineLeftIndendation));
            Console.WriteLine(lines[0].PadRight(maxNumOfChars));
            for (int i = 1; i < lines.Count; i++)
            {
                Console.Write("".PadRight(nextLinesLeftIndendation));
                Console.WriteLine(lines[i].PadRight(maxNumOfChars));
            }
            return lines.Count;
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
