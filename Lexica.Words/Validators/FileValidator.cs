using Lexica.Core.Models;
using Lexica.Core.Validators;
using Lexica.Words.Validators.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lexica.Words.Validators
{
    public class FileValidator : IValidator<string, string>
    {
        public FileValidator(ValidationData validationData)
        {
            ValidationData = validationData;
        }

        public ValidationData ValidationData { get; }

        public enum ElementType
        {
            Word,
            Translation
        };

        public OperationResult Validate(string data, string fileName)
        {
            var result = new OperationResult();

            List<string> lines = data.Split("\n").Select(x => x.Trim()).ToList();
            if (lines.Count == 0)
            {
                result.AddError(
                    new Error(
                        (int)Words.Models.ErrorCodesEnum.FileIsEmpty,
                        $"File {fileName} doesn't exit."
                    )
                );
            }
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                if (line.Length == 0)
                {
                    continue;
                }
                var errorInfo = new Dictionary<string, string>()
                {
                    { "fileName", fileName },
                    { "line", (i + 1).ToString() }
                };
                if (line.IndexOf(";") == -1)
                {
                    result.AddError(
                        new Error<Dictionary<string, string>>(
                            (int)Words.Models.ErrorCodesEnum.NoSemicolon,
                            $"There is no semicolon in file {fileName}, line {i + 1}.",
                            errorInfo
                        )
                    );
                    continue;
                }
                string[] lineParts = line.Split(';');
                if (lineParts.Length > 2)
                {
                    result.AddError(
                        new Error<Dictionary<string, string>>(
                            (int)Words.Models.ErrorCodesEnum.TooManySemicolons,
                            $"There are too many semicolons in file {fileName}, line {i + 1}. There should be only 1 semicolons.",
                            errorInfo
                        )
                    );
                    continue;
                }
                ValidateLinePart(lineParts[0], errorInfo, ElementType.Word, result);
                ValidateLinePart(lineParts[1], errorInfo, ElementType.Translation, result);
            }

            return result;
        }

        private void ValidateLinePart(
            string linePart, 
            Dictionary<string, string> errorInfo, 
            ElementType elementType, 
            OperationResult result)
        {
            List<string> words = linePart.Split(',').Select(x => x.Trim()).ToList<string>();
            var wordValidator = new StringValidator(ValidationData.EntryWord);
            foreach (var word in words)
            {
                var wordValidationResult = wordValidator.Validate(word);
                if (!wordValidationResult.Result && wordValidationResult.Errors != null)
                {
                    foreach (Error wordValidationError in wordValidationResult.Errors)
                    {
                        result.AddError(
                            new Error<Dictionary<string, string>>(
                                (int)MapErrorCode((Core.Models.ErrorCodesEnum)wordValidationError.Code, elementType),
                                wordValidationError.Message,
                                errorInfo
                            )
                        );
                    }
                }
            }
        }

        private Words.Models.ErrorCodesEnum MapErrorCode(Core.Models.ErrorCodesEnum from, ElementType elementType)
        {
            switch (from)
            {
                case Core.Models.ErrorCodesEnum.IsTooShort:
                    switch (elementType)
                    {
                        case ElementType.Word:
                            return Words.Models.ErrorCodesEnum.TooShortWord;
                        case ElementType.Translation:
                            return Words.Models.ErrorCodesEnum.TooShortTranslation;
                    }
                    break;
                case Core.Models.ErrorCodesEnum.IsTooLong:
                    switch (elementType)
                    {
                        case ElementType.Word:
                            return Words.Models.ErrorCodesEnum.TooLongWord;
                        case ElementType.Translation:
                            return Words.Models.ErrorCodesEnum.TooLongTranslation;
                    }
                    break;
            }
            throw new Exception("Unsupported error code.");
        }
    }
}
