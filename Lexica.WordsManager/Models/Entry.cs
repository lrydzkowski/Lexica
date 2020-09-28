using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.WordsManager.Models
{
    class Entry
    {
        public int Id { get; set; }

        public int SetId { get; set; }

        public List<string> Words { get; set; }

        public List<string> Translations { get; set; }
    }
}
