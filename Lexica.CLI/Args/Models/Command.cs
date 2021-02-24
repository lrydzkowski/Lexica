using System.Collections.Generic;

namespace Lexica.CLI.Args.Models
{
    class Command
    {
        public string Name { get; set; } = "";

        public string Shortcut { get; set; } = "";

        public bool HasValue { get; set; } = false;

        public string? ExecutorClass { get; set; } = null;

        public List<Command> Commands { get; set; } = new List<Command>();
    }
}
