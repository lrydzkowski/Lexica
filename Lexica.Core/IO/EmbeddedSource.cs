using Lexica.Core.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Exceptions;
using System.Text;

namespace Lexica.Core.IO
{
    public class EmbeddedSource : ISource
    {
        public string Path { get; private set; }

        public Assembly Assembly { get; private set; }

        public string? Contents { get; private set; }

        public EmbeddedSource(string path, Assembly? assembly = null)
        {
            Path = path;
            Assembly = assembly ?? Assembly.GetExecutingAssembly();
        }

        public string GetContents(bool upToDate = false)
        {
            if (Contents == null || upToDate)
            {
                var resourceLoader = new ResourceLoader();
                Contents = resourceLoader.GetEmbeddedResourceString(Assembly, Path);
                if (Contents == null)
                {
                    throw new ResourceNotFoundException($"Embedded resource {Path} doesn't exist.");
                }
            }

            return Contents;
        }
    }
}
