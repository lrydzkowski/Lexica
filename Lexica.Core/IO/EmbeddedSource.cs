﻿using System.Reflection;
using System.Reflection.Exceptions;

namespace Lexica.Core.IO
{
    public class EmbeddedSource : ISource
    {
        public string Name { get; }

        public string Path { get; }

        public string Namespace
        {
            get
            {
                return System.IO.Path.GetDirectoryName(Path)?.Replace("\\", "/") + "/" ?? Path;
            }
        }

        public Assembly Assembly { get; }

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