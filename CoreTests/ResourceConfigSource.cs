using Lexica.Core.Config;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CoreTests
{
    class ResourceConfigSource : IConfigSource
    {
        public string Path { get; private set; }

        public ResourceConfigSource(string path)
        {
            Path = path;
        }

        public string GetContents()
        {
            var resourceLoader = new ResourceLoader();
            string contents = resourceLoader.GetEmbeddedResourceString(Assembly.GetExecutingAssembly(), Path);
            if (contents == null)
            {
                throw new Exception(String.Format("Resource {0} doesn't exist.", Path));
            }

            return contents;
        }
    }
}
