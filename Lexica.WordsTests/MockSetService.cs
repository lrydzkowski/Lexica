using Lexica.Core.Models;
using Lexica.Words.Models;
using Lexica.Words.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.WordsTests
{
    class MockSetService : ISetService
    {
        public Task<OperationResult> ChangePath(long setId, SetPath newPath)
        {
            throw new NotImplementedException();
        }

        public Task CreateSet(Set set)
        {
            throw new NotImplementedException();
        }

        public Task CreateSet(List<Set> sets)
        {
            throw new NotImplementedException();
        }

        public Task<Set?> Get(long setId)
        {
            throw new NotImplementedException();
        }

        public Task<Set?> Get(List<long> setIds)
        {
            throw new NotImplementedException();
        }

        public Task<List<SetInfo>> GetInfoList()
        {
            throw new NotImplementedException();
        }

        public Task Remove(long setId)
        {
            throw new NotImplementedException();
        }
    }
}
