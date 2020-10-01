using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lexica.Words.Models
{
    public class Set
    {
        public string Id
        {
            get
            {
                return string.Join("-", SetIds);
            }
        }

        public List<long> SetIds { get; set; }

        public SetPath Path { get; set; }

        public List<Entry> Entries { get; set; }
    }
}
