using System.Collections.Generic;

namespace Lexica.Core.IO
{
    public interface IMultipleSource
    {
        public List<ISource> GetContents();
    }
}