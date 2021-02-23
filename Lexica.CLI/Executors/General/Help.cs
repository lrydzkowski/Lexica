﻿using System;

namespace Lexica.CLI.Executors.General
{
    class Help : IExecutor
    {
        public void Execute()
        {
            Console.WriteLine("Info:    English learning application.");
            Console.WriteLine("Usage:   Lexica [OPTIONS] [COMMANDS]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -h  | --help      Show help.");
            Console.WriteLine("  -v  | --version   Show info about version.");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("  i   | import    Import sets.");
            Console.WriteLine("  ls  | list      Show list of sets.");
            Console.WriteLine("  wls | wlist     List of words in set or sets");
            Console.WriteLine("  r   | run       Run application.");
        }
    }
}