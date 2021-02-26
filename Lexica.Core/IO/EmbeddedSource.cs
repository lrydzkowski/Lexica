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
        public string Name { get; private set; }

        public string Path { get; private set; }

        public string Namespace
        {
            get
            {
                return System.IO.Path.GetDirectoryName(Path)?.Replace("\\", "/") + "/" ?? Path;
            }
        }

        public Assembly Assembly { get; private set; }

        public string? Contents { get; private set; }

        public EmbeddedSource(string path, Assembly? assembly = null)
        {
            Path = path.Replace("\\", "/");
            Name = System.IO.Path.GetFileName(Path);
            Assembly = assembly ?? Assembly.GetExecutingAssembly();
        }

        public string GetContents(bool upToDate = false)
        {
            if (Contents == null || upToDate)
            {
                var resourceLoader = new ResourceLoader();
                Contents = resourceLoader.GetEmbeddedResourceString(Assembly, Path.Replace("/", "."));
                if (Contents == null)
                {
                    throw new ResourceNotFoundException($"Embedded resource {Path} doesn't exist.");
                }
            }

            return Contents;
        }
    }
}
