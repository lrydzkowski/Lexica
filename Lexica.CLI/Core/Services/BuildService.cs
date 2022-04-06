using Lexica.Core.IO;
using System;
using System.IO;

namespace Lexica.CLI.Core.Services
{
    internal class BuildService : IService
    {
        public string GetBuild()
        {
            string buildFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "build");
            if (!File.Exists(buildFilePath))
            {
                return "";
            }
            var source = new FileSource(buildFilePath);
            return source.GetContents(true) ?? "";
        }
    }
}