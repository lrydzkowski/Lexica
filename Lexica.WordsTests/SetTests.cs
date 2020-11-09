using Lexica.Core.Models;
using Lexica.Words;
using Lexica.Words.Models;
using Lexica.Words.Validators;
using Lexica.Words.Validators.Models;
using Lexica.WordsTests.Mocks;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Lexica.WordsTests
{
    public class SetTests
    {
        public static IEnumerable<object[]> ChangeSetPathValidationErrorsParameters()
        {
            return new List<object[]>
            {
                new object[]
                {
                    new SetPath(setNamespace: "Other1", name: ""),
                    new List<Core.Models.ErrorCodesEnum>()
                    {
                        Core.Models.ErrorCodesEnum.IsTooShort
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() {
                            { "Name", "SetName" },
                            { "Value", "" }
                        }
                    }
                },
                new object[]
                {
                    new SetPath(setNamespace: "", name: "SomeName"),
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
                    new SetPath(
                        setNamespace: "NamespaceName", 
                        name: "100100100100100100100100100100100100100100100100100100100100100100100100100" +
                        "100100100100100100100100100100100100100100100100100100100100100100100100100"
                    ),
                    new List<Core.Models.ErrorCodesEnum>()
                    {
                        Core.Models.ErrorCodesEnum.IsTooLong
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() {
                            { "Name", "SetName" },
                            { "Value", "100100100100100100100100100100100100100100100100100100100100100100100100100" + 
                            "100100100100100100100100100100100100100100100100100100100100100100100100100" }
                        }
                    }
                },
                new object[]
                {
                    new SetPath(
                        setNamespace: "400400400400400400400400400400400400400400400400400400400400400400400400400" +
                        "400400400400400400400400400400400400400400400400400400400400400400400400400" +
                        "400400400400400400400400400400400400400400400400400400400400400400400400400" +
                        "400400400400400400400400400400400400400400400400400400400400400400400400400" +
                        "400400400400400400400400400400400400400400400400400400400400400400400400400" +
                        "400400400400400400400400400400400400400400400400400400400400400400400400400",
                        name: "SetName"
                    ),
                    new List<Core.Models.ErrorCodesEnum>()
                    {
                        Core.Models.ErrorCodesEnum.IsTooLong
                    },
                    new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() {
                            { "Name", "Namespace" },
                            { "Value", "400400400400400400400400400400400400400400400400400400400400400400400400400" +          
                                "400400400400400400400400400400400400400400400400400400400400400400400400400" +
                                "400400400400400400400400400400400400400400400400400400400400400400400400400" +
                                "400400400400400400400400400400400400400400400400400400400400400400400400400" +
                                "400400400400400400400400400400400400400400400400400400400400400400400400400" +
                                "400400400400400400400400400400400400400400400400400400400400400400400400400" }
                        }
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(ChangeSetPathValidationErrorsParameters))]
        public async void ChangeSetPath_ValidationError_ReturnsFalseResult(
           SetPath newPath,
           List<Core.Models.ErrorCodesEnum> errorCodes,
           List<Dictionary<string, string>> errorDetails)
        {
            // Arrange
            var mockSetService = new MockSetService();
            var setPathValidator = new SetPathValidator(new ValidationData());
            var setManager = new SetManager(mockSetService, 0, setPathValidator);

            // Act
            OperationResult result = await setManager.ChangePath(newPath);

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
