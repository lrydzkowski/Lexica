using System.Collections.Generic;

namespace Lexica.CLI.Args.Models
{
    internal class Command
    {
        public string Name { get; set; } = "";

        public string Shortcut { get; set; } = "";

        public string? ExecutorClass { get; set; } = null;

        public string? DefaultExecutorClass { get; set; } = null;

        public bool? AddNameToArguments { get; set; } = null;

        public List<Command> Commands { get; set; } = new List<Command>();

        public List<string> Parameters { get; set; } = new List<string>();
    }
}