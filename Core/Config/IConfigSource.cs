using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Core.Config
{
    public interface IConfigSource
    {
        public string GetContents();
    }
}
