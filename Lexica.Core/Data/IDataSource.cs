using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Data
{
    public interface IDataSource<T> where T : class
    {
        public T? Get();
    }
}
