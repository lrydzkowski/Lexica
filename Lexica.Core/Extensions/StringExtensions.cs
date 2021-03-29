using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

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
            return char.ToUpper(str[0]) + str.Substring(1);
        }

        public static string RemoveInvalidFileNameChars(this string str)
        {
            return string.Join("", str.Split(Path.GetInvalidFileNameChars()));
        }

        public static bool OrdinalContains(this string s, string value, bool ignoreCase = false)
        {
            return s != null 
                && s.IndexOf(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) >= 0;
        }

        public static bool OrdinalStartsWith(this string s, string value, bool ignoreCase = false)
        {
            return s != null 
                && s.StartsWith(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        public static bool OrdinalEndsWith(this string s, string value, bool ignoreCase = false)
        {
            return s != null 
                && s.EndsWith(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        } 
    }
}
