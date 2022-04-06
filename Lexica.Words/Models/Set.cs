using System.Collections.Generic;

namespace Lexica.Words.Models
{
    public class Set
    {
        public List<SetPath> SetsPaths { get; }

        public List<Entry> Entries { get; }

        public Set(SetPath setPath, List<Entry> entries) : this(new List<SetPath> { setPath }, entries)
        {
        }

        public Set(List<SetPath> setsPaths, List<Entry> entries)
        {
            SetsPaths = setsPaths;
            Entries = entries;
        }
    }
}