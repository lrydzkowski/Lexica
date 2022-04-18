using System.Collections.Generic;
using System.Linq;

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

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                var entry = (Entry)obj;
                return entry.Id == Id
                    && ListsEquals(entry.Words, Words)
                    && ListsEquals(entry.Translations, Translations);
            }
        }

        private static bool ListsEquals(List<string> list1, List<string> list2)
        {
            IEnumerable<string> isFirstOnly = list1.Except(list2);
            IEnumerable<string> isSecondOnly = list2.Except(list1);
            return !isFirstOnly.Any() && !isSecondOnly.Any();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return ToString(EntryPartEnum.Words) + ';' + ToString(EntryPartEnum.Translations);
        }

        public string ToString(EntryPartEnum part)
        {
            return part switch
            {
                EntryPartEnum.Words => string.Join(", ", Words),
                EntryPartEnum.Translations => string.Join(", ", Translations),
                _ => "",
            };
        }
    }
}