using Lexica.Core.Models;
using Lexica.Core.Validators;
using Lexica.Words.Models;
using Lexica.Words.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lexica.Words
{
    public class Importer
    {
        public ISetService SetService { get; }

        public IValidator<Set> SetValidator { get; }

        public Importer(ISetService setService, IValidator<Set> setValidator)
        {
            SetService = setService;
            SetValidator = setValidator;
        }

        public async Task<OperationResult> Import(List<Set> sets)
        {
            var result = new OperationResult();
            foreach (Set set in sets)
            {
                result.Merge(SetValidator.Validate(set));
            }
            if (result.Result)
            {
                await SetService.CreateSet(sets);
            }

            return result;
        }

        public async Task<OperationResult> Import(Set set)
        {
            return await Import(new List<Set>() { set });
        }
    }
}
