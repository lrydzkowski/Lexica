using Lexica.Core.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.CLI.Core.Services
{
    class BuildService : IService
    {
        public string GetBuild()
        {
            var source = new FileSource("build");
            return source.GetContents(true) ?? "";
        }
    }
}
