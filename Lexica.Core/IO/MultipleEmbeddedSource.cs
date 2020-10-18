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
            Path = path;
            Assembly = assembly ?? Assembly.GetExecutingAssembly();
        }

        public List<ISource> GetContents()
        {
            List<ISource> sourceList = new List<ISource>();
            string[] listOfResources = Assembly.GetManifestResourceNames();
            foreach (string resourceName in listOfResources)
            {
                if (resourceName.IndexOf(Path) == 0)
                {
                    var embeddedSource = new EmbeddedSource(resourceName, Assembly);
                    sourceList.Add(embeddedSource);
                }
            }

            return sourceList;
        }
    }
}
