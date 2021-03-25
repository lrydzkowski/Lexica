using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Lexica.Core.Extensions
{
    static class StringExtensions
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
    }
}
