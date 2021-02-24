using Lexica.Core.IO;
using Lexica.Words;
using Lexica.Words.Services;
using Lexica.Words.Validators;
using Lexica.Words.Validators.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Lexica.WordsTests
{
    public class SetModeOperatorTests
    {
        [Theory]
        [InlineData("Lexica.WordsTests/Resources/Importer/Embedded/Correct/example_set_1.txt")]
        [InlineData("Lexica.WordsTests/Resources/Importer/Embedded/Correct/example_set_2.txt")]
        public void RandomizeEntries_ProperConditions_ReturnsEntriesInRandomOrder(string filePath)
        {
            var fileValidator = new FileValidator(new ValidationData());
            var setService = new SetService(fileValidator);
            var fileSource = new EmbeddedSource(filePath, Assembly.GetExecutingAssembly());
            var setModeOperator = new SetModeOperator(setService, fileSource);

            setModeOperator.LoadSet();
            if (setModeOperator.Set == null)
            {
                throw new Exception("Set is null.");
            }

            setModeOperator.Reset();
            setModeOperator.Randomize();
            var str1 = "";
            var entry1 = setModeOperator.GetNextEntry();
            while (entry1 != null)
            {
                str1 += string.Join(',', entry1.Words) + ',' + string.Join(',', entry1.Translations);
                entry1 = setModeOperator.GetNextEntry();
            }

            setModeOperator.Reset();
            setModeOperator.Randomize();
            var str2 = "";
            var entry2 = setModeOperator.GetNextEntry();
            while (entry2 != null)
            {
                str2 += string.Join(',', entry2.Words) + ',' + string.Join(',', entry2.Translations);
                entry2 = setModeOperator.GetNextEntry();
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
            var setModeOperator = new SetModeOperator(setService, fileSource);

            setModeOperator.LoadSet();
            if (setModeOperator.Set == null)
            {
                throw new Exception("Set is null.");
            }

            setModeOperator.Reset();
            setModeOperator.Randomize();
            int index = 0;
            var entry = setModeOperator.GetNextEntry();
            while (entry != null)
            {
                entry = setModeOperator.GetNextEntry();
                index++;
            }

            Assert.Equal(numOfLines, index);
        }
    }
}
