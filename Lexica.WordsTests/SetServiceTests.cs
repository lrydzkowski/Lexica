using Lexica.Core.IO;
using Lexica.Core.Models;
using Lexica.Words.Services;
using Lexica.Words.Validators;
using Lexica.Words.Validators.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Exceptions;
using Xunit;

namespace Lexica.WordsTests
{
    public class SetServiceTests
    {
        [Theory]
        [InlineData("Lexica.WordsTests/Resources/Importer/Embedded/Correct2/example_set_1.txt")]
        public void LoadSet_NotExistedFile_ReturnsFalseResult(string filePath)
        {
            // Arrange
            var fileValidator = new FileValidator(new ValidationData());
            var setService = new SetService(fileValidator);
            var fileSource = new EmbeddedSource(filePath, Assembly.GetExecutingAssembly());

            // Act
            void action() => setService.Load(fileSource);

            // Assert
            ResourceNotFoundException ex = Assert.Throws<ResourceNotFoundException>(action);

        }

        public static IEnumerable<object[]> LoadSetValidationErrorsParameters()
        {
            return new List<object[]>
            {
                new object[]
                {
                    new List<string>()
                    {
                        "Lexica.WordsTests/Resources/Importer/Embedded/TooManySemicolons/too_many_semicolons_1.txt",
                    },
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
                    new List<string>()
                    {
                        "Lexica.WordsTests/Resources/Importer/Embedded/NoSemicolon/no_semicolon_1.txt",
                        "Lexica.WordsTests/Resources/Importer/Embedded/NoSemicolon/no_semicolon_2.txt"
                    },
                    new List<Enum>()
                    {
                        Words.Models.ErrorCodesEnum.NoSemicolon,
                        Words.Models.ErrorCodesEnum.NoSemicolon,
                        Words.Models.ErrorCodesEnum.NoSemicolon
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() { { "Line", "17" }, { "FileName", "no_semicolon_1.txt" } },
                        new Dictionary<string, string>() { { "Line", "21" }, { "FileName", "no_semicolon_1.txt" } },
                        new Dictionary<string, string>() { { "Line", "2" }, { "FileName", "no_semicolon_2.txt" } }
                    }
                },
                new object[]
                {
                    new List<string>()
                    {
                        "Lexica.WordsTests/Resources/Importer/Embedded/TooLongWord/too_long_word_1.txt"
                    },
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
                    new List<string>()
                    {
                        "Lexica.WordsTests/Resources/Importer/Embedded/TooLongTranslation/too_long_translation_1.txt"
                    },
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
                    new List<string>()
                    {
                        "Lexica.WordsTests/Resources/Importer/Embedded/TooShortWord/too_short_word_1.txt",
                    },
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
                    new List<string>()
                    {
                        "Lexica.WordsTests/Resources/Importer/Embedded/TooShortTranslation/too_short_translation_1.txt"
                    },
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
                    new List<string>()
                    {
                        "Lexica.WordsTests/Resources/Importer/Embedded/ErrorsCombination/example_set_1.txt",
                        "Lexica.WordsTests/Resources/Importer/Embedded/ErrorsCombination/example_set_2.txt",
                        "Lexica.WordsTests/Resources/Importer/Embedded/ErrorsCombination/example_set_3.txt"
                    },
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
        [MemberData(nameof(LoadSetValidationErrorsParameters))]
        public void LoadSet_ValidationErrors_ReturnsFalseResult(
            List<string> filesPaths, 
            List<Enum> errorCodes, 
            List<Dictionary<string, string>> errorDetails)
        {
            // Arrange
            var fileValidator = new FileValidator(new ValidationData());
            var setService = new SetService(fileValidator);
            var filesSources = new List<ISource>();
            foreach (string filePath in filesPaths)
            {
                filesSources.Add(new EmbeddedSource(filePath, Assembly.GetExecutingAssembly()));
            }

            // Act
            OperationResult result = setService.Load(filesSources);

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
