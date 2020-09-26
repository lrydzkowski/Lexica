using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Core.IO
{
    public interface ISource
    {
        public string GetContents();
    }
}
