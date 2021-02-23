using System;
using System.Reflection;

namespace Lexica.CLI.Executors.General
{
    class Version : IExecutor
    {
        public void Execute()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "";
            string build = "";
            Console.WriteLine($"Lexica ${version} (CLI) (build: ${build}");
            Console.WriteLine("Copyright © Łukasz Rydzkowski");
        }
    }
}
