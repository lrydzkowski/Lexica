using Lexica.Core.IO;
using Lexica.Core.Models;
using Lexica.Words;
using Lexica.Words.Validators;
using Lexica.Words.Validators.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Lexica.WordsTests
{
    public class ImporterTests
    {
        [Theory]
        [InlineData("Lexica.WordsTests/Resources/Importer/Embedded/Correct2")]
        public async void Import_NotExistedDir_ReturnsFalseResult(string dirPath)
        {
            // Arrange
            var mockSetService = new MockSetService();
            var setValidator = new SetValidator(new ValidationData());
            var fileValidator = new FileValidator(new ValidationData());
            var importer = new Importer(mockSetService, setValidator, fileValidator);
            var multipleSource = new MultipleEmbeddedSource(dirPath, Assembly.GetExecutingAssembly());

            // Act
            OperationResult result = await importer.Import(multipleSource);

            // Assert
            Assert.False(result.Result);
            Assert.Equal(1, result.Errors?.Count);
            Assert.Equal((int)Lexica.Words.Models.ErrorCodesEnum.NoImportSource, result.Errors?[0].Code);
        }

        public static IEnumerable<object[]> ImportValidationErrorsParameters()
        {
            return new List<object[]>
            {
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/TooManySemicolons",
                    new List<Words.Models.ErrorCodesEnum>()
                    {
                        Words.Models.ErrorCodesEnum.TooManySemicolons,
                        Words.Models.ErrorCodesEnum.TooManySemicolons,
                        Words.Models.ErrorCodesEnum.TooManySemicolons
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() { { "line", "14" }, { "fileName", "too_many_semicolons_1.txt" } },
                        new Dictionary<string, string>() { { "line", "15" }, { "fileName", "too_many_semicolons_1.txt" } },
                        new Dictionary<string, string>() { { "line", "16" }, { "fileName", "too_many_semicolons_1.txt" } }
                    }
                },
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/NoSemicolon",
                    new List<Words.Models.ErrorCodesEnum>()
                    {
                        Words.Models.ErrorCodesEnum.NoSemicolon,
                        Words.Models.ErrorCodesEnum.NoSemicolon
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() { { "line", "17" }, { "fileName", "no_semicolon_1.txt" } },
                        new Dictionary<string, string>() { { "line", "21" }, { "fileName", "no_semicolon_1.txt" } }
                    }
                },
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/TooLongWord",
                    new List<Words.Models.ErrorCodesEnum>()
                    {
                        Words.Models.ErrorCodesEnum.TooLongWord
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() { { "line", "30" }, { "fileName", "too_long_word_1.txt" } }
                    }
                },
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/TooLongTranslation",
                    new List<Words.Models.ErrorCodesEnum>()
                    {
                        Words.Models.ErrorCodesEnum.TooLongTranslation
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() { { "line", "22" }, { "fileName", "too_long_translation_1.txt" } }
                    }
                },
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/TooShortWord",
                    new List<Words.Models.ErrorCodesEnum>()
                    {
                        Words.Models.ErrorCodesEnum.TooShortWord,
                        Words.Models.ErrorCodesEnum.TooShortWord
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() { { "line", "24" }, { "fileName", "too_short_word_1.txt" } },
                        new Dictionary<string, string>() { { "line", "31" }, { "fileName", "too_short_word_1.txt" } }
                    }
                },
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/TooShortTranslation",
                    new List<Words.Models.ErrorCodesEnum>()
                    {
                        Words.Models.ErrorCodesEnum.TooShortTranslation,
                        Words.Models.ErrorCodesEnum.TooShortTranslation
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() { { "line", "1" }, { "fileName", "too_short_translation_1.txt" } },
                        new Dictionary<string, string>() { { "line", "20" }, { "fileName", "too_short_translation_1.txt" } }
                    }
                },
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/ErrorsCombination",
                    new List<Words.Models.ErrorCodesEnum>()
                    {
                        Words.Models.ErrorCodesEnum.TooShortTranslation,
                        Words.Models.ErrorCodesEnum.TooShortWord,
                        Words.Models.ErrorCodesEnum.NoSemicolon,
                        Words.Models.ErrorCodesEnum.TooManySemicolons,
                        Words.Models.ErrorCodesEnum.TooLongTranslation,
                        Words.Models.ErrorCodesEnum.TooLongWord,
                        Words.Models.ErrorCodesEnum.TooLongTranslation
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() { { "line", "21" }, { "fileName", "example_set_1.txt" } },
                        new Dictionary<string, string>() { { "line", "25" }, { "fileName", "example_set_1.txt" } },
                        new Dictionary<string, string>() { { "line", "19" }, { "fileName", "example_set_2.txt" } },
                        new Dictionary<string, string>() { { "line", "23" }, { "fileName", "example_set_2.txt" } },
                        new Dictionary<string, string>() { { "line", "25" }, { "fileName", "example_set_2.txt" } },
                        new Dictionary<string, string>() { { "line", "13" }, { "fileName", "example_set_3.txt" } },
                        new Dictionary<string, string>() { { "line", "18" }, { "fileName", "example_set_3.txt" } }
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(ImportValidationErrorsParameters))]
        public async void Import_ValidationErrors_ReturnsFalseResult(
            string dirPath, 
            List<Words.Models.ErrorCodesEnum> errorCodes, 
            List<Dictionary<string, string>> errorDetails)
        {
            // Arrange
            var mockSetService = new MockSetService();
            var setValidator = new SetValidator(new ValidationData());
            var fileValidator = new FileValidator(new ValidationData());
            var importer = new Importer(mockSetService, setValidator, fileValidator);
            var multipleSource = new MultipleEmbeddedSource(dirPath, Assembly.GetExecutingAssembly());

            // Act
            OperationResult result = await importer.Import(multipleSource);

            // Assert
            Assert.False(result.Result);
            if (result.Errors == null)
            {
                throw new Exception("There are no expected errors.");
            }
            Assert.Equal(errorCodes.Count, result.Errors.Count);
            for (int i = 0; i < result.Errors.Count; i++)
            {
                if (result.Errors[i] is Error<Dictionary<string, string>> error)
                {
                    Assert.Equal((int)errorCodes[i], error.Code);
                    foreach (KeyValuePair<string, string> el in errorDetails[i])
                    {
                        Assert.True(error.Data.ContainsKey(el.Key));
                        Assert.Equal(el.Value, error.Data[el.Key]);
                    }
                }
                else
                {
                    throw new Exception("Error is not Error<Dictionary<string, string>> type.");
                }
            }
        }
    }
}
