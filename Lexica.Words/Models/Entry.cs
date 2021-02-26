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
                return $"{SetPath.Namespace}{SetPath.Name}:{LineNum}";
            }
        }

        public SetPath SetPath { get; }

        public int LineNum { get; }

        public List<string> Words { get; }

        public List<string> Translations { get; }

        public Entry(SetPath setPath, int lineNum, List<string> words, List<string> translations)
        {
            SetPath = setPath;
            LineNum = lineNum;
            Words = words;
            Translations = translations;
        }
    }
}
