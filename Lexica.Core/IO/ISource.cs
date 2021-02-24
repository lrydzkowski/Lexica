using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Core.IO
{
    public interface ISource
    {
        public string Name { get; }

        public string Path { get; }

        public string GetContents(bool upToDate = false);
    }
}
