using Lexica.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Core.IO
{
    public class FileSource : ISource
    {
        public string Path { get; private set; }

        public FileSource(string path)
        {
            Path = path;
            if (!File.Exists(Path))
            {
                throw new FileNotFoundException(String.Format("File {0} doesn't exist.", Path));
            }
        }

        public string GetContents()
        {
            return File.ReadAllText(Path);
        }
    }
}
