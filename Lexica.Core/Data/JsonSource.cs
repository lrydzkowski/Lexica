using Lexica.Core.IO;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lexica.Core.Data
{
    class JsonSource<T> : IDataSource<T> where T : class
    {
        private ISource Source { get; set; }

        private T? Data { get; set; }

        public JsonSource(ISource source)
        {
            Source = source;
            Load();
        }

        public T? Load()
        {
            string contents = Source.GetContents();
            Data = JsonSerializer.Deserialize<T>(contents);

            return Data;
        }

        public T? Get()
        {
            if (Data == null)
            {
                return Load();
            }
            return Data;
        }
    }
}
