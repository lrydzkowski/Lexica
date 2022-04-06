using Lexica.Core.IO;
using Lexica.Core.Models;
using Lexica.Words.Models;
using System.Collections.Generic;

namespace Lexica.Words.Services
{
    public interface ISetService
    {
        public OperationResult<Set?> Load(List<ISource> filesSources);
    }
}