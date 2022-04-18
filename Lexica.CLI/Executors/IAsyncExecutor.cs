using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lexica.CLI.Executors
{
    internal interface IAsyncExecutor
    {
        Task ExecuteAsync(List<string>? args = null);
    }
}