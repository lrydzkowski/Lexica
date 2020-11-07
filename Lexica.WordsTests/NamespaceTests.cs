using Lexica.Core.Models;
using Lexica.Core.Validators;
using Lexica.Words;
using Lexica.Words.Validators.Models;
using Lexica.WordsTests.Mocks;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Lexica.WordsTests
{
    public class NamespaceTests
    {
        public static IEnumerable<object[]> ChangeNamespacePathValidationErrorsParameters()
        {
            return new List<object[]>
            {
                new object[]
                {
                    "",
                    "Other2",
                    new List<Core.Models.ErrorCodesEnum>()
                    {
                        Core.Models.ErrorCodesEnum.IsTooShort
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() {
                            { "Name", "Namespace" }, 
                            { "Value", "" }
                        }
                    }
                },
                new object[]
                {
                    "Other1",
                    "",
                    new List<Core.Models.ErrorCodesEnum>()
                    {
                        Core.Models.ErrorCodesEnum.IsTooShort
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() {
                            { "Name", "Namespace" },
                            { "Value", "" }
                        }
                    }
                },
                new object[]
                {
                    "Other1",
                    "400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400" +
                    "400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400" +
                    "400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400" +
                    "400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400" +
                    "400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400",
                    new List<Core.Models.ErrorCodesEnum>()
                    {
                        Core.Models.ErrorCodesEnum.IsTooLong
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() {
                            { "Name", "Namespace" },
                            { "Value", "400400400400400400400400400400400400400400400400400400400400400400400400400" + "400400400400400400400400400400400400400400400400400400400400400400400400400400400400" + "400400400400400400400400400400400400400400400400400400400400400400400400400400400400" + "400400400400400400400400400400400400400400400400400400400400400400400400400400400400" + "400400400400400400400400400400400400400400400400400400400400400400400400400400400400" +
                              "400400400400400400400400400400400400400400400400400400" }
                        }
                    }
                },
                new object[]
                {
                    "400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400" +
                    "400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400" +
                    "400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400" +
                    "400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400" +
                    "400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400400",
                    "Other2",
                    new List<Core.Models.ErrorCodesEnum>()
                    {
                        Core.Models.ErrorCodesEnum.IsTooLong
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() {
                            { "Name", "Namespace" },
                            { "Value", "400400400400400400400400400400400400400400400400400400400400400400400400400" + "400400400400400400400400400400400400400400400400400400400400400400400400400400400400" + "400400400400400400400400400400400400400400400400400400400400400400400400400400400400" + "400400400400400400400400400400400400400400400400400400400400400400400400400400400400" + "400400400400400400400400400400400400400400400400400400400400400400400400400400400400" +
                              "400400400400400400400400400400400400400400400400400400" }
                        }
                    }
                },
                new object[]
                {
                    "",
                    "",
                    new List<Core.Models.ErrorCodesEnum>()
                    {
                        Core.Models.ErrorCodesEnum.IsTooShort,
                        Core.Models.ErrorCodesEnum.IsTooShort
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() {
                            { "Name", "Namespace" },
                            { "Value", "" }
                        },
                        new Dictionary<string, string>() {
                            { "Name", "Namespace" },
                            { "Value", "" }
                        }
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(ChangeNamespacePathValidationErrorsParameters))]
        public async void ChangeNamespacePath_ValidationError_ReturnsFalseResult(
            string from, 
            string to, 
            List<Core.Models.ErrorCodesEnum> errorCodes, 
            List<Dictionary<string, string>> errorDetails)
        {
            // Arrange
            var mockNamespaceService = new MockNamespaceService();
            var validationData = new ValidationData();
            var namespaceValidator = new StringValidator(validationData.Namespace);
            var namespaceManager = new NamespaceManager(mockNamespaceService, namespaceValidator);

            // Act
            OperationResult result = await namespaceManager.ChangeNamespacePath(from, to);

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
