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
        private string _filePath = "";
        public string FilePath 
        {
            get
            {
                return _filePath;
            }
            private set
            {
                if (!File.Exists(value))
                {
                    throw new FileNotFoundException($"File {value} doesn't exist.");
                }
                _filePath = value;
            }
        }

        public string? Contents { get; private set; }

        public FileSource(string filePath)
        {
            FilePath = filePath;
        }

        public string GetContents(bool upToDate = false)
        {
            if (Contents == null || upToDate)
            {
                Contents = File.ReadAllText(FilePath);
            }

            return Contents;
        }
    }
}
