using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.WordsManager.Models
{
    class Set
    {
        public long Id { get; set; }

        public SetPath Name { get; set; }

        public List<Entry> Entries { get; set; }
    }
}
