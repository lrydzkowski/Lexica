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

        public EmbeddedSource(string path, Assembly assembly = null)
        {
            Path = path;
            Assembly = assembly ?? Assembly.GetExecutingAssembly();
        }

        public string GetContents()
        {
            var resourceLoader = new ResourceLoader();
            string contents = resourceLoader.GetEmbeddedResourceString(Assembly, Path);
            if (contents == null)
            {
                throw new ResourceNotFoundException(String.Format("Embedded resource {0} doesn't exist.", Path));
            }

            return contents;
        }
    }
}
