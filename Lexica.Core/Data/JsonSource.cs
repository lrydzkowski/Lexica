using System.Text.Json;
using Lexica.Core.IO;

namespace Lexica.Core.Data
{
    internal class JsonSource<T> : IDataSource<T> where T : class
    {
        private ISource Source { get; }

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
            return Data ?? Load();
        }
    }
}