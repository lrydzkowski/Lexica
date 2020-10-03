using Lexica.Core.Models;
using Lexica.Words.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Words.Services
{
    public interface ISetService
    {
        public Task CreateSet(Set set);

        public Task CreateSet(List<Set> sets);

        public Task<Set> Get(long setId);

        public Task<Set> Get(List<long> setIds);

        public Task<List<SetInfo>> GetInfoList();

        public Task<OperationResult> ChangePath(long setId, SetPath newPath);

        public Task Remove(long setId);
    }
}
