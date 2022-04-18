using System.Collections.Generic;
using Lexica.Core.IO;
using Lexica.Core.Models;
using Lexica.Words.Models;

namespace Lexica.Words.Services
{
    public interface ISetService
    {
        public OperationResult<Set?> Load(List<ISource> filesSources);
    }
}