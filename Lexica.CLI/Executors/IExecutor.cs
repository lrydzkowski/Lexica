using System.Collections.Generic;

namespace Lexica.CLI.Executors
{
    interface IExecutor
    {
        void Execute(List<string>? args = null);
    }
}
