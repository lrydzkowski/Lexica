using Lexica.Core.Models;
using Lexica.LearningMode.Models;
using Lexica.Words.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Lexica.LearningModeTests
{
    public class QuestionInfoTests
    {
        public static IEnumerable<object?[]> GetCompareQuestionInfoEqualObjectsParameters()
        {
            return new List<object?[]>
            {
                new object?[]
                {
                    new Entry(
                        new SetPath("C:/Users/lukas/Documents/Lexica", "set_1.txt"),
                        2,
                        new List<string>() { "word1", "word2" },
                        new List<string>() { "translation1", "translation2" }
                    ),
                    QuestionTypeEnum.Closed,
                    ModeTypeEnum.Translations,
                    null,
                    null
                },
                new object?[]
                {
                    new Entry(
                        new SetPath("C:/Users/lukas/Documents/Lexica", "set_1.txt"),
                        2,
                        new List<string>() { "word1", "word2" },
                        new List<string>() { "translation1", "translation2" }
                    ),
                    QuestionTypeEnum.Open,
                    ModeTypeEnum.Words,
                    new List<string> { "test1", "test2" },
                    new List<string> { "test2", "test1" }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetCompareQuestionInfoEqualObjectsParameters))]
        public void CompareQuestionInfo_EqualObjects_ReturnsTrue(
            Entry entry,
            QuestionTypeEnum questionType,
            ModeTypeEnum modeType,
            List<string>? possibleAnswers1,
            List<string>? possibleAnswers2)
        {
            // Arrange
            var questionInfo1 = new QuestionInfo(entry, questionType, modeType, possibleAnswers1);
            var questionInfo2 = new QuestionInfo(entry, questionType, modeType, possibleAnswers2 );

            // Act
            bool result = questionInfo1.Equals(questionInfo2);

            // Assert
            Assert.True(result);
        }

        public static IEnumerable<object?[]> GetCompareQuestionInfoNotEqualObjectsParameters()
        {
            return new List<object?[]>
            {
                new object?[]
                {
                    new Entry(
                        new SetPath("C:/Users/lukas/Documents/Lexica", "set_1.txt"),
                        2,
                        new List<string>() { "word1", "word2" },
                        new List<string>() { "translation1", "translation2" }
                    ),
                    new Entry(
                        new SetPath("C:/Users/lukas/Documents/Lexica", "set_1.txt"),
                        2,
                        new List<string>() { "word1", "word2" },
                        new List<string>() { "translation1", "translation2" }
                    ),
                    QuestionTypeEnum.Closed,
                    QuestionTypeEnum.Open,
                    ModeTypeEnum.Translations,
                    ModeTypeEnum.Translations,
                    null,
                    null
                },
                new object?[]
                {
                    new Entry(
                        new SetPath("C:/Users/lukas/Documents/Lexica", "set_1.txt"),
                        2,
                        new List<string>() { "word1", "word2" },
                        new List<string>() { "translation1", "translation2" }
                    ),
                    new Entry(
                        new SetPath("C:/Users/lukas/Documents/Lexica", "set_1.txt"),
                        2,
                        new List<string>() { "word1", "word2" },
                        new List<string>() { "translation1", "translation2" }
                    ),
                    QuestionTypeEnum.Open,
                    QuestionTypeEnum.Open,
                    ModeTypeEnum.Translations,
                    ModeTypeEnum.Words,
                    null,
                    null
                },
                new object?[]
                {
                    new Entry(
                        new SetPath("C:/Users/lukas/Documents/Lexica", "set_1.txt"),
                        2,
                        new List<string>() { "word1", "word2" },
                        new List<string>() { "translation1", "translation2" }
                    ),
                    new Entry(
                        new SetPath("C:/Users/lukas/Documents/Lexica", "set_1.txt"),
                        2,
                        new List<string>() { "word1", "word2" },
                        new List<string>() { "translation1", "translation2" }
                    ),
                    QuestionTypeEnum.Open,
                    QuestionTypeEnum.Open,
                    ModeTypeEnum.Translations,
                    ModeTypeEnum.Translations,
                    new List<string> { "test2" },
                    null
                },
                new object?[]
                {
                    new Entry(
                        new SetPath("C:/Users/lukas/Documents/Lexica", "set_1.txt"),
                        2,
                        new List<string>() { "word1", "word2" },
                        new List<string>() { "translation1", "translation2" }
                    ),
                    new Entry(
                        new SetPath("C:/Users/lukas/Documents/Lexica", "set_1.txt"),
                        2,
                        new List<string>() { "word1", "word2" },
                        new List<string>() { "translation1", "translation2" }
                    ),
                    QuestionTypeEnum.Open,
                    QuestionTypeEnum.Open,
                    ModeTypeEnum.Translations,
                    ModeTypeEnum.Translations,
                    new List<string> { "test2" },
                    new List<string> { "test2", "test1" },
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetCompareQuestionInfoNotEqualObjectsParameters))]
        public void CompareQuestionInfo_NotEqualObjects_ReturnsTrue(
            Entry entry1,
            Entry entry2,
            QuestionTypeEnum questionType1,
            QuestionTypeEnum questionType2,
            ModeTypeEnum modeType1,
            ModeTypeEnum modeType2,
            List<string>? possibleAnswers1,
            List<string>? possibleAnswers2)
        {
            // Arrange
            var questionInfo1 = new QuestionInfo(entry1, questionType1, modeType1, possibleAnswers1);
            var questionInfo2 = new QuestionInfo(entry2, questionType2, modeType2, possibleAnswers2);

            // Act
            bool result = questionInfo1.Equals(questionInfo2);

            // Assert
            Assert.False(result);
        }
    }
}
