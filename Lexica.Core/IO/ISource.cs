namespace Lexica.Core.IO
{
    public interface ISource
    {
        public string Name { get; }

        public string Path { get; }

        public string Namespace { get; }

        public string GetContents(bool upToDate = false);
    }
}