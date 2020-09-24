using Lexica.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Core.Config
{
    class FileConfigSource : IConfigSource
    {
        public string Path { get; private set; }

        public FileConfigSource(string path)
        {
            if (File.Exists(path))
            {
                throw new WrongConfigException("Config file doesn't exist.");
            }
            Path = path;
        }

        public string GetContents()
        {
            return File.ReadAllText(Path);
        }

        public Stream GetContentsStream()
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(Path) ?? ""));
        }
    }
}
