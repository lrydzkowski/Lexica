using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Words.Models
{
    public class Entry
    {
        public string Id
        {
            get
            {
                return string.Format("{0}-{1}", SetId, EntryId);
            }
        }

        public long SetId { get; set; }

        public int EntryId { get; set; }

        public List<string> Words { get; set; }

        public List<string> Translations { get; set; }
    }
}
