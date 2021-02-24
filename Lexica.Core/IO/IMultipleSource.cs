using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.IO
{
    public interface IMultipleSource
    {
        public List<ISource> GetContents();
    }
}
