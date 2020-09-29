﻿using Lexica.Core.Models;
using Lexica.Words.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Words.Services
{
    public interface ISetService
    {
        public Task<Set> Get(long setId);

        public Task<Set> Get(List<long> setIds);

        public Task<List<Set>> GetList(bool includeEntries = false);

        public Task<OperationResult> ChangePath(long setId, SetPath newPath);

        public Task<OperationResult> Remove(long setId);
    }
}
