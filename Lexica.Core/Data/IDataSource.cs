namespace Lexica.Core.Data
{
    public interface IDataSource<T> where T : class
    {
        public T? Get();
    }
}