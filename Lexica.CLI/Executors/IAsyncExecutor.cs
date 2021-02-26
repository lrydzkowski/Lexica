using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lexica.CLI.Executors
{
    interface IAsyncExecutor
    {
        Task ExecuteAsync(List<string>? args = null);
    }
}
