using Lexica.Core.Models;
using Lexica.MaintainingMode;
using Lexica.MaintainingMode.Models;
using Lexica.MaintainingMode.Config;
using Lexica.MaintainingModeTests.Mocks;
using Lexica.Words;
using Lexica.Words.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

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
                    "test1,test2",
                    ModeTypeEnum.Translations,
                    false
                },
                new object[]
                {
                    "test1,test2",
                    ModeTypeEnum.Words,
                    false
                },
                new object[]
                {
                    "nieporównywalnie",
                    ModeTypeEnum.Translations,
                    true
                },
                new object[]
                {
                    "incomparably",
                    ModeTypeEnum.Words,
                    true
                }
            };
        }

        [Theory]
        [MemberData(nameof(VerifyAnswerWrongAnswerParameters))]
        public async Task VerifyAnswer_WrongAnswer_ReturnsProperAnswerResult(
            string wrongAnswer, 
            ModeTypeEnum modeType, 
            bool result)
        {
            // Arrange
            var setService = new MockSetService();
            var setModeOperator = new SetModeOperator(setService, 1);
            await setModeOperator.LoadSet();
            var modeManager = new Manager(
                setModeOperator, modeType, new MaintainingSettings() { ResetAfterMistake = null }
            );
            Entry? entry = await setModeOperator.GetEntry(1, 1);
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
            Question? question = await modeManager.GetQuestion();
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

        [Fact]
        public async Task QuestionsRandomness_CorrectData_QuestionsInRandomOrder()
        {
            // Arrange
            var setService = new MockSetService();
            var setModeOperator = new SetModeOperator(setService, 1);
            await setModeOperator.LoadSet();
            var modeManager = new Manager(
                setModeOperator, 
                ModeTypeEnum.Words, 
                new MaintainingSettings() { ResetAfterMistake = null }
            );

            // Act
            await modeManager.Randomize();
            List<string> questionsContent = new List<string>();
            Question? question = await modeManager.GetQuestion();
            while (question != null)
            {
                questionsContent.Add(question.Content);
                question = await modeManager.GetQuestion();
            }
            await modeManager.Randomize();
            List<string> questionsAfterRandomizeContent = new List<string>();
            question = await modeManager.GetQuestion();
            while (question != null)
            {
                questionsAfterRandomizeContent.Add(question.Content);
                question = await modeManager.GetQuestion();
            }

            // Assert
            Assert.NotEqual(string.Join(',', questionsContent), string.Join(',', questionsAfterRandomizeContent));
        }
    }
}
