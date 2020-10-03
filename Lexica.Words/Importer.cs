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

        public async Task Import(List<Set> sets)
        {
            await SetService.CreateSet(sets);
        }

        public async Task Import(Set set)
        {
            await Import(new List<Set>() { set });
        }
    }
}
