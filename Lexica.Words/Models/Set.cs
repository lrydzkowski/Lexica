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

        public List<SetInfo> SetsInfo { get; }

        public List<Entry> Entries { get; }

        public Set(SetInfo setInfo, List<Entry> entries) : this(new List<SetInfo> { setInfo }, entries) { }

        public Set(List<SetInfo> setsInfo, List<Entry> entries)
        {
            SetsInfo = setsInfo;
            Entries = entries;
        }
    }
}
