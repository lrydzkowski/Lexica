using System;
using System.Collections.Generic;

namespace Lexica.CLI.Executors.General
{
    internal class Help : IExecutor
    {
        public void Execute(List<string>? args = null)
        {
            Console.WriteLine("Info:    English learning application for Windows.");
            Console.WriteLine("Usage:   Lexica.CLI.exe [command]");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("  -h  | --help      Show help.");
            Console.WriteLine("  -v  | --version   Show info about version.");
            Console.WriteLine("  r   | run         Run application.");
        }
    }
}