using Lexica.Core.Models;
using Lexica.Core.Validators;
using Lexica.Words.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Words
{
    public class NamespaceManager
    {
        public INamespaceService NamespaceService { get; }

        public IValidator<string> Validator { get; }

        public NamespaceManager(INamespaceService namespaceService, IValidator<string> validator)
        {
            NamespaceService = namespaceService;
            Validator = validator;
        }

        public async Task<OperationResult> ChangeNamespacePath(string oldNamespacePath, string newNamespacePath)
        {
            var result = new OperationResult();
            result.Merge(Validator.Validate(oldNamespacePath));
            result.Merge(Validator.Validate(newNamespacePath));
            if (result.Result)
            {
                result.Merge(await NamespaceService.ChangePath(oldNamespacePath, newNamespacePath));
            }

            return result;
        }
    }
}
