using Lexica.Words.Models;
using Lexica.Words.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Words
{
    public class Importer
    {
        ISetService SetService { get; set; }

        public Importer(ISetService setService)
        {
            SetService = setService;
        }

        public void Import(List<Set> sets)
        {
            SetService.CreateSet(sets);
        }

        public void Import(Set set)
        {
            Import(new List<Set>() { set });
        }
    }
}
