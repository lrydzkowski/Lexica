using Lexica.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lexica.Core.IO
{
    public class MultipleEmbeddedSource : IMultipleSource
    {
        public string Path { get; private set; }

        public Assembly Assembly { get; private set; }

        public MultipleEmbeddedSource(string path, Assembly? assembly = null)
        {
            Path = path.Replace("\\", "/");
            Assembly = assembly ?? Assembly.GetExecutingAssembly();
        }

        public List<ISource> GetContents()
        {
            List<ISource> sourceList = new();
            string[] listOfResources = Assembly.GetManifestResourceNames();
            string path = Path.Replace("/", ".");
            foreach (string resourceName in listOfResources)
            {
                if (resourceName.IndexOf(path) == 0)
                {
                    string[] rnParts = resourceName.Split('.');
                    string fileName = rnParts[^2] + '.' + rnParts[^1];
                    var resourceDirPath = resourceName.ReplaceLastOccurence(fileName, "").Replace(".", "/");
                    var resourceFullPath = resourceDirPath + fileName;
                    var embeddedSource = new EmbeddedSource(resourceFullPath, Assembly);
                    sourceList.Add(embeddedSource);
                }
            }

            return sourceList;
        }
    }
}
