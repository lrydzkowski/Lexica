using Lexica.CLI.Executors;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lexica.CLI.Modes.Spelling
{
    class SpellingModeExecutor : IAsyncExecutor
    {
        public async Task ExecuteAsync(List<string>? args = null)
        {
            Console.WriteLine("Spelling mode :)");
        }
    }
}
