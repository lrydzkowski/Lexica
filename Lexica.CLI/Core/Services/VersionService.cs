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
            string version = GetType().Assembly.GetName().Version?.ToString() ?? "";
            List<string> versionParts = version.Split('.').ToList();
            versionParts.RemoveAt(3);
            string cutVersion = string.Join('.', versionParts);
            return cutVersion;
        }
    }
}
