using Lexica.Core.IO;
using Lexica.Words;
using Lexica.Words.Models;
using Lexica.Words.Services;
using Lexica.Words.Validators;
using Lexica.Words.Validators.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Lexica.WordsTests
{
    public class WordsSetOperatorTests
    {
        [Theory]
        [InlineData("Lexica.WordsTests/Resources/Importer/Embedded/Correct/example_set_1.txt")]
        [InlineData("Lexica.WordsTests/Resources/Importer/Embedded/Correct/example_set_2.txt")]
        public void RandomizeEntries_ProperConditions_ReturnsEntriesInRandomOrder(string filePath)
        {
            var fileValidator = new FileValidator(new ValidationData());
            var setService = new SetService(fileValidator);
            var fileSource = new EmbeddedSource(filePath, Assembly.GetExecutingAssembly());
            var wordsSetOperator = new WordsSetOperator(setService, fileSource);

            if (!wordsSetOperator.LoadSet())
            {
                throw new Exception("Set is null.");
            }

            string str1 = "";
            foreach (Entry? entry1 in wordsSetOperator.GetEntries(false, true))
            {
                if (entry1 == null)
                {
                    break;
                }
                str1 += string.Join(',', entry1.Words) + ',' + string.Join(',', entry1.Translations);
            }

            string str2 = "";
            foreach (Entry? entry2 in wordsSetOperator.GetEntries(false, true))
            {
                if (entry2 == null)
                {
                    break;
                }
                str1 += string.Join(',', entry2.Words) + ',' + string.Join(',', entry2.Translations);
            }

            Assert.NotEqual(str1, str2);
        }

        public static IEnumerable<object[]> GetAllEntriesProperConditionsParameters()
        {
            return new List<object[]>
            {
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/Correct/example_set_3.txt",
                    27
                },
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/Correct/example_set_4.txt",
                    22
                },
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/Correct/example_set_5.txt",
                    28
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetAllEntriesProperConditionsParameters))]
        public void GetAllEntries_ProperConditions_ReturnsAllEntries(string filePath, int numOfLines)
        {
            var fileValidator = new FileValidator(new ValidationData());
            var setService = new SetService(fileValidator);
            var fileSource = new EmbeddedSource(filePath, Assembly.GetExecutingAssembly());
            var wordsSetOperator = new WordsSetOperator(setService, fileSource);

            if (!wordsSetOperator.LoadSet())
            {
                throw new Exception("Set is null.");
            }

            int index = 0;
            foreach (Entry? entry in wordsSetOperator.GetEntries(false, true))
            {
                if (entry == null)
                {
                    break;
                }
                index++;
            }

            Assert.Equal(numOfLines, index);
        }
    }
}