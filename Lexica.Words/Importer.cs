using Lexica.Core.Models;
using Lexica.Words.Models;
using Lexica.Words.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Words
{
    public class Importer
    {
        ISetService SetService { get; set; }

        public Importer(ISetService setService)
        {
            SetService = setService;
        }

        public async Task<OperationResult> Import(List<Set> sets)
        {
            var result = new OperationResult();
            await SetService.CreateSet(sets);
            return result;
        }

        public async Task<OperationResult> Import(Set set)
        {
            return await Import(new List<Set>() { set });
        }
    }
}
