using Lexica.Core.Models;
using Lexica.MaintainingMode;
using Lexica.MaintainingMode.Models;
using Lexica.MaintainingMode.Config;
using Lexica.Words;
using Lexica.Words.Models;
using System;
using System.Collections.Generic;
using Xunit;
using Lexica.Words.Services;
using Lexica.Words.Validators;
using Lexica.Words.Validators.Models;
using System.Reflection;
using Lexica.Core.IO;

namespace Lexica.MaintainingModeTests
{
    public class ManagerTests
    {
        public static IEnumerable<object[]> VerifyAnswerWrongAnswerParameters()
        {
            return new List<object[]>
            {
                new object[]
                {
                    "Lexica.MaintainingModeTests/Resources/Embedded/Correct/example_set_1.txt",
                    "test1,test2",
                    ModeTypeEnum.Translations,
                    false
                },
                new object[]
                {
                    "Lexica.MaintainingModeTests/Resources/Embedded/Correct/example_set_1.txt",
                    "test1,test2",
                    ModeTypeEnum.Words,
                    false
                },
                new object[]
                {
                    "Lexica.MaintainingModeTests/Resources/Embedded/Correct/example_set_1.txt",
                    "nieporównywalnie",
                    ModeTypeEnum.Translations,
                    true
                },
                new object[]
                {
                    "Lexica.MaintainingModeTests/Resources/Embedded/Correct/example_set_1.txt",
                    "incomparably",
                    ModeTypeEnum.Words,
                    true
                }
            };
        }

        [Theory]
        [MemberData(nameof(VerifyAnswerWrongAnswerParameters))]
        public void VerifyAnswer_WrongAnswer_ReturnsProperAnswerResult(
            string filePath,
            string wrongAnswer, 
            ModeTypeEnum modeType, 
            bool result)
        {
            // Arrange
            var fileValidator = new FileValidator(new ValidationData());
            var setService = new SetService(fileValidator);
            var fileSource = new EmbeddedSource(filePath, Assembly.GetExecutingAssembly());
            var setModeOperator = new SetModeOperator(setService, fileSource);
            var modeManager = new Manager(
                setModeOperator, modeType, new MaintainingSettings() { ResetAfterMistake = null }
            );
            Entry? entry = setModeOperator.GetEntry(fileSource.Namespace, fileSource.Name, 1);
            if (entry == null)
            {
                throw new Exception("Entry is null");
            }
            List<string> properAnswers = new List<string>();
            switch (modeType)
            {
                case ModeTypeEnum.Translations:
                    properAnswers = entry.Translations;
                    break;
                case ModeTypeEnum.Words:
                    properAnswers = entry.Words;
                    break;
            }

            // Act
            Question? question = modeManager.GetQuestion(false, false);
            AnswerResult? answerResult = modeManager.VerifyAnswer(wrongAnswer);
            if (answerResult == null)
            {
                throw new Exception("Answer result is null.");
            }

            // Assert
            Assert.Equal(answerResult.Result, result);
            Assert.Equal(
                string.Join(',', answerResult.PossibleAnswers), 
                string.Join(',', properAnswers)
            );
        }

        public static IEnumerable<object[]> CheckQuestionsRandomnessCorrectDataParameters()
        {
            return new List<object[]>
            {
                new object[]
                {
                    "Lexica.MaintainingModeTests/Resources/Embedded/Correct/example_set_1.txt",
                    ModeTypeEnum.Translations
                }
            };
        }

        [Theory]
        [MemberData(nameof(CheckQuestionsRandomnessCorrectDataParameters))]
        public void CheckQuestionsRandomness_CorrectData_QuestionsInRandomOrder(
            string filePath, 
            ModeTypeEnum modeType)
        {
            // Arrange
            var fileValidator = new FileValidator(new ValidationData());
            var setService = new SetService(fileValidator);
            var fileSource = new EmbeddedSource(filePath, Assembly.GetExecutingAssembly());
            var setModeOperator = new SetModeOperator(setService, fileSource);
            var modeManager = new Manager(
                setModeOperator, modeType, new MaintainingSettings() { ResetAfterMistake = null }
            );

            // Act
            modeManager.Randomize();
            List<string> questionsContent = new List<string>();
            Question? question = modeManager.GetQuestion();
            for (int i = 0, ii = modeManager.GetNumberOfQuestions(); i < ii; i++)
            {
                var rnd = new Random();
                questionsContent.Add(question?.Content ?? rnd.Next(0, 100).ToString());
                question = modeManager.GetQuestion();
            }
            modeManager.Randomize();
            List<string> questionsAfterRandomizeContent = new List<string>();
            question = modeManager.GetQuestion();
            for (int i = 0, ii = modeManager.GetNumberOfQuestions(); i < ii; i++)
            {
                var rnd = new Random();
                questionsContent.Add(question?.Content ?? rnd.Next(0, 100).ToString());
                question = modeManager.GetQuestion();
            }

            // Assert
            Assert.NotEqual(string.Join(',', questionsContent), string.Join(',', questionsAfterRandomizeContent));
        }
    }
}
