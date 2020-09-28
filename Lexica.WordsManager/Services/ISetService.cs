using Lexica.Core.Models;
using Lexica.WordsManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.WordsManager.Services
{
    interface ISetService
    {
        public Set GetSet(int setId);

        public OperationResult ChangeSetPath(SetPath oldSetPath, SetPath newSetPath);

        public OperationResult ChangeNamespaceName(string oldNamespaceName, string newNamespaceName);
    }
}
