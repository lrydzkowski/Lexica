﻿using Lexica.Core.Models;
using Lexica.Words.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.WordsTests.Mocks
{
    class MockNamespaceService : INamespaceService
    {
        public Task<OperationResult> ChangePath(string oldNamespacePath, string newNamespacePath)
        {
            throw new NotImplementedException();
        }
    }
}