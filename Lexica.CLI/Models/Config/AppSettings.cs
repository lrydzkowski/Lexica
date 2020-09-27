using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.CLI.Models.Config
{
    public class AppSettings
    {
        public Database Database { get; set; }

        public Words Words { get; set; }

        public Maintaining Maintaining { get; set; }
    }
}
