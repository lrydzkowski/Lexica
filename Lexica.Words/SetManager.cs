using Lexica.Core.Models;
using Lexica.Words.Models;
using Lexica.Words.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Words
{
    public class SetManager
    {
        ISetService SetService { get; set; }

        long SetId { get; set; }

        public SetManager(ISetService setService, long setId)
        {
            SetService = setService;
            SetId = setId;
        }

        public async Task<List<Set>> GetList()
        {
            return await SetService.GetList();
        }

        public async Task<OperationResult> ChangePath(SetPath newPath)
        {
            return await SetService.ChangePath(SetId, newPath);
        }

        public async Task<OperationResult> Remove()
        {
            return await SetService.Remove(SetId);
        }
    }
}
