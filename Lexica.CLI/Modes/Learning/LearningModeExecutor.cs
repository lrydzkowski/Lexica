using Lexica.CLI.Executors;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lexica.CLI.Modes.Learning
{
    class LearningModeExecutor : IAsyncExecutor
    {
        public async Task ExecuteAsync(List<string>? args = null)
        {
            Console.WriteLine("Learning mode :)");
        }
    }
}
