using System.Collections.Generic;

namespace Lexica.CLI.Args.Models
{
    internal class CLIMap
    {
        public List<Command> Commands { get; set; } = new List<Command>();
    }
}