using System.Collections.Generic;

namespace Lexica.CLI.Executors
{
    internal interface IExecutor
    {
        void Execute(List<string>? args = null);
    }
}