using System;
using System.Collections.Generic;
using System.Text;
using Lexica.Core.Data;
using Lexica.Core.Models;
using Lexica.Core.Validators;
using Lexica.Words.Models;
using Lexica.Words.Validators;
using Lexica.Words.Validators.Models;

namespace Lexica.EF.Services
{
    class SetValidator : IValidator<Set>
    {
        public ValidationData ValidationData { get; private set; }

        public SetValidator(ValidationData validationData)
        {
            ValidationData = validationData;
        }

        public OperationResult Validate(Set set)
        {
            var result = new OperationResult();
            var setPathValidator = new SetPathValidator(ValidationData);
            foreach (SetInfo setInfo in set.SetsInfo)
            {
                result.Merge(setPathValidator.Validate(setInfo.Path));
            }
            var wordValidator = new StringValidator(ValidationData.EntryWord);
            var translationValidator = new StringValidator(ValidationData.EntryTranslation);
            foreach (Entry entry in set.Entries)
            {
                foreach (string word in entry.Words)
                {
                    result.Merge(wordValidator.Validate(word));
                }
                foreach (string translation in entry.Translations)
                {
                    result.Merge(translationValidator.Validate(translation));
                }
            }
            
            return result;
        }
    }
}
