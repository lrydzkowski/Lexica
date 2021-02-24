using Lexica.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Words.Services
{
    public interface INamespaceService
    {
        public Task<OperationResult> ChangePath(string oldNamespacePath, string newNamespacePath);
    }
}
