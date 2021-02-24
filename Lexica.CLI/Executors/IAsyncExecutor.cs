using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.CLI.Executors
{
    interface IAsyncExecutor
    {
        Task ExecuteAsync();
    }
}
