using Lexica.Words.Models;
using System.Collections.Generic;
using Xunit;

namespace Lexica.WordsTests
{
    public class EntryTests
    {
        public static IEnumerable<object[]> GetCompareEntriesEqualObjectsParameters()
        {
            return new List<object[]>
            {
                new object[]
                {
                    "C:/Users/lukas/Documents/Lexica",
                    "set_1.txt",
                    2,
                    new List<string>() { "word1", "word2" },
                    new List<string>() { "word1", "word2" },
                    new List<string>() { "translation1", "translation2" },
                    new List<string>() { "translation1", "translation2" }
                },
                new object[]
                {
                    "C:/Users/lukas/Documents/Lexica",
                    "set_1.txt",
                    2,
                    new List<string>() { "word1", "word2" },
                    new List<string>() { "word2", "word1" },
                    new List<string>() { "translation1", "translation2" },
                    new List<string>() { "translation2", "translation1" }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetCompareEntriesEqualObjectsParameters))]
        public void CompareEntries_EqualObjects_ReturnsTrue(
            string namespaceName,
            string fileName,
            int lineNum,
            List<string> words1,
            List<string> words2,
            List<string> translations1,
            List<string> translations2)
        {
            // Arrange
            var entry1 = new Entry(
                new SetPath(namespaceName, fileName),
                lineNum,
                words1,
                translations1
            );
            var entry2 = new Entry(
                new SetPath(namespaceName, fileName),
                lineNum,
                words2,
                translations2
            );

            // Act
            bool result = entry1.Equals(entry2);

            // Assert
            Assert.True(result);
        }

        public static IEnumerable<object[]> GetCompareEntriesNotEqualObjectsParameters()
        {
            return new List<object[]>
            {
                new object[]
                {
                    "C:/Users/lukas/Documents/Lexica",
                    "C:/Users/lukas/Documents/Lexica2",
                    "set_1.txt",
                    "set_1.txt",
                    2,
                    2,
                    new List<string>() { "word1", "word2" },
                    new List<string>() { "word1", "word2" },
                    new List<string>() { "translation1", "translation2" },
                    new List<string>() { "translation1", "translation2" }
                },
                new object[]
                {
                    "C:/Users/lukas/Documents/Lexica",
                    "C:/Users/lukas/Documents/Lexica",
                    "set_1.txt",
                    "set_2.txt",
                    2,
                    2,
                    new List<string>() { "word1", "word2" },
                    new List<string>() { "word1", "word2" },
                    new List<string>() { "translation1", "translation2" },
                    new List<string>() { "translation1", "translation2" }
                },
                new object[]
                {
                    "C:/Users/lukas/Documents/Lexica",
                    "C:/Users/lukas/Documents/Lexica",
                    "set_1.txt",
                    "set_1.txt",
                    2,
                    3,
                    new List<string>() { "word1", "word2" },
                    new List<string>() { "word1", "word2" },
                    new List<string>() { "translation1", "translation2" },
                    new List<string>() { "translation1", "translation2" }
                },
                new object[]
                {
                    "C:/Users/lukas/Documents/Lexica",
                    "C:/Users/lukas/Documents/Lexica",
                    "set_1.txt",
                    "set_1.txt",
                    2,
                    2,
                    new List<string>() { "word1", "word2" },
                    new List<string>() { "word1", "word3", "word2" },
                    new List<string>() { "translation1", "translation2" },
                    new List<string>() { "translation1", "translation2" }
                },
                new object[]
                {
                    "C:/Users/lukas/Documents/Lexica",
                    "C:/Users/lukas/Documents/Lexica",
                    "set_1.txt",
                    "set_1.txt",
                    2,
                    2,
                    new List<string>() { "word1", "word2" },
                    new List<string>() { "word1", "word2" },
                    new List<string>() { "translation1", "translation2" },
                    new List<string>() { "translation2", "translation3" }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetCompareEntriesNotEqualObjectsParameters))]
        public void CompareEntries_NotEqualObjects_ReturnsFalse(
            string namespaceName1,
            string namespaceName2,
            string fileName1,
            string fileName2,
            int lineNum1,
            int lineNum2,
            List<string> words1,
            List<string> words2,
            List<string> translations1,
            List<string> translations2)
        {
            // Arrange
            var entry1 = new Entry(
                new SetPath(namespaceName1, fileName1),
                lineNum1,
                words1,
                translations1
            );
            var entry2 = new Entry(
                new SetPath(namespaceName2, fileName2),
                lineNum2,
                words2,
                translations2
            );

            // Act
            bool result = entry1.Equals(entry2);

            // Assert
            Assert.False(result);
        }
    }
}