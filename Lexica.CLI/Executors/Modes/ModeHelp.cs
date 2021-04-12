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
            Console.WriteLine("  s   | spelling        Check your spelling.");
            Console.WriteLine("  f   | full            Full path of learning with closed and open questions.");
            Console.WriteLine("  oo  | only-open       Only open questions.");
        }
    }
}
