using System;
using System.Collections;

namespace Lexica.CLI.Core
{
    static class View
    {
        public static void ShowError(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("An unexpected error has occured:");
            Console.WriteLine();
            Console.WriteLine(ex.ToString());
            if (ex.Data.Count > 0)
            {
                Console.WriteLine("System.Exception.Data:");
                foreach (DictionaryEntry? entry in ex.Data)
                {
                    Console.WriteLine(String.Format("   {0}: {1}", entry?.Key, entry?.Value));
                }
            }
            Console.ResetColor();
        }

        public static void ShowError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
