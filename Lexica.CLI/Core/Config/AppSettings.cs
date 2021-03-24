using Lexica.EF.Config;
using Lexica.LearningMode.Config;
using Lexica.MaintainingMode.Config;
using Lexica.Pronunciation.Forvo.Config;
using Lexica.Words.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.CLI.Core.Config
{
    public class AppSettings
    {
        public DatabaseSettings? Database { get; set; }

        public WordsSettings? Words { get; set; }

        public MaintainingSettings? Maintaining { get; set; }

        public LearningSettings? Learning { get; set; }

        public ForvoSettings? ForvoSettings { get; set; }
    }
}
