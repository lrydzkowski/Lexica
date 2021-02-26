using System;
using System.Collections.Generic;

namespace Lexica.CLI.Executors.Modes
{
    class ModeHelp : IExecutor
    {
        public void Execute(List<string>? args = null)
        {
            Console.WriteLine("Info:    Run learning mode.");
            Console.WriteLine("Usage:   Lexica run [COMMANDS]");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("  -m  | --mode          Run mode.");
            Console.WriteLine();
            Console.WriteLine("Available modes:");
            Console.WriteLine("  m   | maintaining     Maintaining mode.");
            Console.WriteLine("  l   | learning        Learning mode.");
            Console.WriteLine("  m   | spelling        Spelling mode.");
        }
    }
}
