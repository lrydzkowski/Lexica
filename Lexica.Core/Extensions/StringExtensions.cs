using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lexica.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceLastOccurence(this string str, string find, string replace)
        {
            int place = str.LastIndexOf(find);
            if (place == -1)
            {
                return str;
            }
            string result = str.Remove(place, find.Length).Insert(place, replace);

            return result;
        }

        public static string UppercaseFirst(this string str)
        {
            return char.ToUpper(str[0]) + str[1..];
        }

        public static string RemoveInvalidFileNameChars(this string str)
        {
            return string.Concat(str.Split(Path.GetInvalidFileNameChars()));
        }

        public static bool OrdinalContains(this string str, string value, bool ignoreCase = false)
        {
            return str?.Contains(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == true;
        }

        public static bool OrdinalStartsWith(this string str, string value, bool ignoreCase = false)
        {
            return str?.StartsWith(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == true;
        }

        public static bool OrdinalEndsWith(this string str, string value, bool ignoreCase = false)
        {
            return str?.EndsWith(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == true;
        }

        public static List<string> Split(this string str, int chunkLength, bool wholeWords = true)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("String cannot be null or empty.");
            }
            if (chunkLength < 1)
            {
                throw new ArgumentException("Chunk length cannot be shorter than 1.");
            }
            List<string> result = new();

            if (wholeWords)
            {
                if (str.Length > chunkLength)
                {
                    StringBuilder sb = new();
                    string[] words = str.Split(' ');
                    foreach (string word in words)
                    {
                        if (sb.Length != 0)
                        {
                            sb.Append(' ');
                        }
                        if (word.Length > chunkLength)
                        {
                            List<string> wordParts = word.Split(chunkLength, false);
                            result.AddRange(wordParts);
                            continue;
                        }
                        if (sb.Length + word.Length > chunkLength)
                        {
                            result.Add(sb.ToString());
                            sb.Clear();
                        }
                        sb.Append(word);
                    }
                    if (sb.Length > 0)
                    {
                        result.Add(sb.ToString());
                    }
                }
                else
                {
                    result.Add(str);
                }
            }
            else
            {
                for (int i = 0; i < str.Length; i += chunkLength)
                {
                    if (chunkLength + i > str.Length)
                    {
                        chunkLength = str.Length - i;
                    }
                    result.Add(str.Substring(i, chunkLength));
                }
            }

            return result;
        }
    }
}