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
        public string Name { get; private set; }

        private string _path = "";
        public string Path 
        {
            get
            {
                return _path;
            }
            private set
            {
                if (!File.Exists(value))
                {
                    throw new FileNotFoundException($"File {value} doesn't exist.");
                }
                _path = value;
            }
        }

        public string Namespace
        {
            get
            {
                return System.IO.Path.GetDirectoryName(Path)?.Replace("\\", "/") + "/" ?? Path;
            }
        }

        public string? Contents { get; private set; }

        public FileSource(string filePath)
        {
            Path = filePath.Replace("\\", "/");
            Name = System.IO.Path.GetFileName(filePath);
        }

        public string GetContents(bool upToDate = false)
        {
            if (Contents == null || upToDate)
            {
                Contents = File.ReadAllText(Path);
            }

            return Contents;
        }
    }
}
