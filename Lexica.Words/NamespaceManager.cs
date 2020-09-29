using Lexica.Core.Models;
using Lexica.Words.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Words
{
    public class NamespaceManager
    {
        INamespaceService NamespaceService { get; set; }

        public NamespaceManager(INamespaceService namespaceService)
        {
            NamespaceService = namespaceService;
        }

        public async Task<OperationResult> ChangeNamespacePath(string oldNamespacePath, string newNamespacePath)
        {
            return await NamespaceService.ChangePath(oldNamespacePath, newNamespacePath);
        }
    }
}
