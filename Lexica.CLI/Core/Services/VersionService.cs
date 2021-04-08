using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.CLI.Core.Services
{
    class VersionService : IService
    {
        public string GetVersion()
        {
            return GetType().Assembly.GetName().Version?.ToString() ?? "";
        }
    }
}
