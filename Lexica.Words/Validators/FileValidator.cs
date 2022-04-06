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

        public OperationResult Validate(string data, string fileName)
        {
            var result = new OperationResult();

            List<string> lines = data.Split("\n").Select(x => x.Trim()).ToList();
            if (lines.Count == 0)
            {
                result.AddError(
                    new Error(
                        (int)Words.Models.ErrorCodesEnum.FileIsEmpty,
                        $"File '{fileName}' is empty."
                    )
                );
            }
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                if (line.Length == 0) continue;
                int lineNumber = i + 1;
                OperationResult lineContainsSemicolonResult = LineContainsSemicolon(line, fileName, lineNumber);
                result.Merge(lineContainsSemicolonResult);
                if (!lineContainsSemicolonResult.Result) continue;
                OperationResult lineHasToManySemicolonResult = LineHasToManySemicolons(line, fileName, lineNumber);
                result.Merge(lineHasToManySemicolonResult);
                if (!lineHasToManySemicolonResult.Result) continue;
                result.Merge(ValidateWords(line, fileName, lineNumber));
                result.Merge(ValidateTranslations(line, fileName, lineNumber));
            }

            return result;
        }

        private OperationResult LineContainsSemicolon(string line, string fileName, int lineNumber)
        {
            OperationResult result = new();
            if (!line.Contains(';', StringComparison.CurrentCulture))
            {
                result.AddError(
                    new Error<Dictionary<string, string>>(
                        (int)Words.Models.ErrorCodesEnum.NoSemicolon,
                        $"There is no semicolon in file {fileName}, line {lineNumber}.",
                        GetErrorData(fileName, lineNumber)
                    )
                );
            }
            return result;
        }

        private OperationResult LineHasToManySemicolons(string line, string fileName, int lineNumber)
        {
            OperationResult result = new();
            string[] lineParts = line.Split(';');
            if (lineParts.Length > 2)
            {
                result.AddError(
                    new Error<Dictionary<string, string>>(
                        (int)Words.Models.ErrorCodesEnum.TooManySemicolons,
                        $"There are too many semicolons in file {fileName}, line {lineNumber}."
                            + " There should be only one semicolon.",
                        GetErrorData(fileName, lineNumber)
                    )
                );
            }
            return result;
        }

        private OperationResult ValidateWords(string line, string fileName, int lineNumber)
        {
            OperationResult result = new();
            string[] lineParts = line.Split(';');
            if (lineParts.Length == 0) return result;
            List<string> words = lineParts[0].Split(',').Select(x => x.Trim()).ToList<string>();
            var wordValidator = new StringValidator(ValidationData.EntryWord);
            foreach (var word in words)
            {
                var wordValidatorResult = wordValidator.Validate(word);
                wordValidatorResult.AddDictionaryDataToError(GetErrorData(fileName, lineNumber));
                result.Merge(wordValidatorResult);
            }
            return result;
        }

        private OperationResult ValidateTranslations(string line, string fileName, int lineNumber)
        {
            OperationResult result = new();
            string[] lineParts = line.Split(';');
            if (lineParts.Length < 2) return result;
            List<string> translations = lineParts[1].Split(',').Select(x => x.Trim()).ToList<string>();
            var translationValidator = new StringValidator(ValidationData.EntryTranslation);
            foreach (var translation in translations)
            {
                var translationValidatorResult = translationValidator.Validate(translation);
                translationValidatorResult.AddDictionaryDataToError(GetErrorData(fileName, lineNumber));
                result.Merge(translationValidatorResult);
            }
            return result;
        }

        private Dictionary<string, string> GetErrorData(string fileName, int line)
        {
            var errorInfo = new Dictionary<string, string>()
            {
                { "FileName", fileName },
                { "Line", line.ToString() }
            };
            return errorInfo;
        }
    }
}