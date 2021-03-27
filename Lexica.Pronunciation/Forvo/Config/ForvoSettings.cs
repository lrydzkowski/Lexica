using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Pronunciation.Forvo.Config
{
    public class ForvoSettings
    {
        public string Key { get; set; } = "";

        public string Language { get; set; } = "";

        public string Country { get; set; } = "";

        public string Order { get; set; } = "";

        public int Limit { get; set; } = 0;

        public string DownloadTempPath { get; set; } = "";
    }
}
