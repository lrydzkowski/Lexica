using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.CLI.Executors.Modes
{
    class SpellingMode : IAsyncExecutor
    {
        public async Task ExecuteAsync(List<string>? args = null)
        {
            Console.WriteLine("Learning mode :)");
        }
    }
}
