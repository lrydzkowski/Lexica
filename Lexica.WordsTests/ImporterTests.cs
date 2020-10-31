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
            var importer = new Importer(mockSetService, fileValidator);
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
                    new List<Enum>()
                    {
                        Words.Models.ErrorCodesEnum.TooManySemicolons,
                        Words.Models.ErrorCodesEnum.TooManySemicolons,
                        Words.Models.ErrorCodesEnum.TooManySemicolons
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() {
                            { "Line", "14" }, { "FileName", "too_many_semicolons_1.txt" }
                        },
                        new Dictionary<string, string>() {
                            { "Line", "15" }, { "FileName", "too_many_semicolons_1.txt" }
                        },
                        new Dictionary<string, string>() {
                            { "Line", "16" }, { "FileName", "too_many_semicolons_1.txt" }
                        }
                    }
                },
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/NoSemicolon",
                    new List<Enum>()
                    {
                        Words.Models.ErrorCodesEnum.NoSemicolon,
                        Words.Models.ErrorCodesEnum.NoSemicolon
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() { { "Line", "17" }, { "FileName", "no_semicolon_1.txt" } },
                        new Dictionary<string, string>() { { "Line", "21" }, { "FileName", "no_semicolon_1.txt" } }
                    }
                },
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/TooLongWord",
                    new List<Enum>()
                    {
                        Core.Models.ErrorCodesEnum.IsTooLong
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() { 
                            { "Line", "30" }, { "FileName", "too_long_word_1.txt" }, { "Name", "Word" }, 
                            { "Value", "505050505050505050505050505050505050505050505050505050" } 
                        }
                    }
                },
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/TooLongTranslation",
                    new List<Enum>()
                    {
                        Core.Models.ErrorCodesEnum.IsTooLong
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() {
                            { "Line", "22" }, { "FileName", "too_long_translation_1.txt" }, { "Name", "Translation" },
                            { "Value", "505050505050505050505050505050505050505050505050505050" }
                        }
                    }
                },
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/TooShortWord",
                    new List<Enum>()
                    {
                        Core.Models.ErrorCodesEnum.IsTooShort,
                        Core.Models.ErrorCodesEnum.IsTooShort
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() {
                            { "Line", "24" }, { "FileName", "too_short_word_1.txt" }, { "Name", "Word" },
                            { "Value", "" }
                        },
                        new Dictionary<string, string>() {
                            { "Line", "31" }, { "FileName", "too_short_word_1.txt" }, { "Name", "Word" } ,
                            { "Value", "" }
                        }
                    }
                },
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/TooShortTranslation",
                    new List<Enum>()
                    {
                        Core.Models.ErrorCodesEnum.IsTooShort,
                        Core.Models.ErrorCodesEnum.IsTooShort
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() {
                            { "Line", "1" }, { "FileName", "too_short_translation_1.txt" }, { "Name", "Translation" },
                            { "Value", "" }
                        },
                        new Dictionary<string, string>() {
                            { "Line", "20" }, { "FileName", "too_short_translation_1.txt" }, { "Name", "Translation" },
                            { "Value", "" }
                        }
                    }
                },
                new object[]
                {
                    "Lexica.WordsTests/Resources/Importer/Embedded/ErrorsCombination",
                    new List<Enum>()
                    {
                        Core.Models.ErrorCodesEnum.IsTooShort,
                        Core.Models.ErrorCodesEnum.IsTooShort,
                        Words.Models.ErrorCodesEnum.NoSemicolon,
                        Words.Models.ErrorCodesEnum.TooManySemicolons,
                        Core.Models.ErrorCodesEnum.IsTooLong,
                        Core.Models.ErrorCodesEnum.IsTooLong,
                        Core.Models.ErrorCodesEnum.IsTooLong
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() {
                            { "Line", "21" }, { "FileName", "example_set_1.txt" }, { "Name", "Translation" },
                            { "Value", "" }
                        },
                        new Dictionary<string, string>() {
                            { "Line", "25" }, { "FileName", "example_set_1.txt" }, { "Name", "Word" }, { "Value", "" }
                        },
                        new Dictionary<string, string>() { { "Line", "19" }, { "FileName", "example_set_2.txt" } },
                        new Dictionary<string, string>() { { "Line", "23" }, { "FileName", "example_set_2.txt" } },
                        new Dictionary<string, string>() {
                            { "Line", "25" }, { "FileName", "example_set_2.txt" }, { "Name", "Translation" },
                            { "Value", "strona wszczynająca spór sądowy5050505050505sądowy5050505050505sądowy5050505050505sądowy5050505050505" }
                        },
                        new Dictionary<string, string>() {
                            { "Line", "13" }, { "FileName", "example_set_3.txt" }, { "Name", "Word" },
                            { "Value", "WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWstarvation" }
                        },
                        new Dictionary<string, string>() {
                            { "Line", "18" }, { "FileName", "example_set_3.txt" }, { "Name", "Translation" },
                            { "Value", "zuchwałyWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW" }
                        }
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(ImportValidationErrorsParameters))]
        public async void Import_ValidationErrors_ReturnsFalseResult(
            string dirPath, 
            List<Enum> errorCodes, 
            List<Dictionary<string, string>> errorDetails)
        {
            // Arrange
            var mockSetService = new MockSetService();
            var setValidator = new SetValidator(new ValidationData());
            var fileValidator = new FileValidator(new ValidationData());
            var importer = new Importer(mockSetService, fileValidator);
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
                    int errorCode = 0;
                    if (errorCodes[i] is Core.Models.ErrorCodesEnum coreErrorCode)
                    {
                        errorCode = (int)coreErrorCode;
                    }
                    if (errorCodes[i] is Words.Models.ErrorCodesEnum wordErrorCode)
                    {
                        errorCode = (int)wordErrorCode;
                    }
                    Assert.Equal(errorCode, error.Code);
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
