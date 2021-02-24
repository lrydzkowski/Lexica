using Lexica.Core.Models;
using Lexica.Core.Validators;
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
        public ISetService SetService { get; }

        public long SetId { get; }

        public IValidator<SetPath> SetPathValidator { get; }

        public SetManager(ISetService setService, long setId, IValidator<SetPath> setPathValidator)
        {
            SetService = setService;
            SetId = setId;
            SetPathValidator = setPathValidator;
        }

        public async Task<Set?> GetSet()
        {
            return await SetService.Get(SetId);
        }

        public async Task<List<SetInfo>> GetInfoList()
        {
            return await SetService.GetInfoList();
        }

        public async Task<OperationResult> ChangePath(SetPath newPath)
        {
            var operation = new OperationResult();
            operation.Merge(SetPathValidator.Validate(newPath));
            if (operation.Result)
            {
                operation.Merge(await SetService.ChangePath(SetId, newPath));
            }

            return operation;
        }

        public async Task Remove()
        {
            await SetService.Remove(SetId);
        }
    }
}
