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

        public long SetId { get; }

        public int EntryId { get; }

        public List<string> Words { get; }

        public List<string> Translations { get; }

        public Entry(long setId, int entryId, List<string> words, List<string> translations)
        {
            SetId = setId;
            EntryId = entryId;
            Words = words;
            Translations = translations;
        }
    }
}
