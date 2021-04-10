using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lexica.Core.IO
{
    public class MultipleFilesSource : IMultipleSource
    {
        private string _directoryPath = "";
        public string DirectoryPath
        {
            get
            {
                return _directoryPath;
            }
            private set
            {
                if (!Directory.Exists(value))
                {
                    throw new DirectoryNotFoundException($"Directory {value} doesn't exist.");
                }
                _directoryPath = value;
            }
        }

        public MultipleFilesSource(string directoryPath)
        {
            DirectoryPath = directoryPath;
        }

        public List<ISource> GetContents()
        {
            List<ISource> fileSourceList = new();
            DirectoryInfo di = new(DirectoryPath);
            foreach (FileInfo fi in di.EnumerateFiles())
            {
                var fileSource = new FileSource(Path.Combine(DirectoryPath, fi.Name));
                fileSourceList.Add(fileSource);
            }

            return fileSourceList;
        }
    }
}
