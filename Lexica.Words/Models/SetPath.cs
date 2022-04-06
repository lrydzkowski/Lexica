namespace Lexica.Words.Models
{
    public class SetPath
    {
        public string Namespace { get; }

        public string Name { get; }

        public SetPath(string setNamespace, string setName)
        {
            Namespace = setNamespace;
            Name = setName;
        }
    }
}