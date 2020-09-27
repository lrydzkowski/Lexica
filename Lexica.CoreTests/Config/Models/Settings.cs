using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.CoreTests.Config.Models
{
    public class Settings
    {
        public Database Database { get; set; }

        public Words Words { get; set; }

        public Spelling Spelling { get; set; }

        public Learning Learning { get; set; }

        public Maintaining Maintaining { get; set; }
    }
}
