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
                    { "FileName", fileName },
                    { "Line", (i + 1).ToString() }
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

                List<string> words = lineParts[0].Split(',').Select(x => x.Trim()).ToList<string>();
                var wordValidator = new StringValidator(ValidationData.EntryWord);
                foreach (var word in words)
                {
                    var wordValidatorResult = wordValidator.Validate(word);
                    wordValidatorResult.AddDictionaryDataToError(errorInfo);
                    result.Merge(wordValidatorResult);
                }

                List<string> translations = lineParts[1].Split(',').Select(x => x.Trim()).ToList<string>();
                var translationValidator = new StringValidator(ValidationData.EntryTranslation);
                foreach (var translation in translations)
                {
                    var translationValidatorResult = translationValidator.Validate(translation);
                    translationValidatorResult.AddDictionaryDataToError(errorInfo);
                    result.Merge(translationValidatorResult);
                }
            }

            return result;
        }
    }
}
