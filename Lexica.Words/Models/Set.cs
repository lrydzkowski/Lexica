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
                return string.Join("-", SetsInfo.Select(x => x.SetId));
            }
        }

        public List<SetInfo> SetsInfo { get; set; }

        public List<Entry> Entries { get; set; }
    }
}
