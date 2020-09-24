using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Config.Models
{
    public class Settings
    {
        public Database Database { get; set; }

        public Words Words { get; set; }

        public Maintaining Maintaining { get; set; }
    }
}
