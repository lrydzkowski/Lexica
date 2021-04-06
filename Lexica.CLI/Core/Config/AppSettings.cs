using Lexica.EF.Config;
using Lexica.LearningMode.Config;
using Lexica.Pronunciation.Config;
using Lexica.Words.Config;

namespace Lexica.CLI.Core.Config
{
    public class AppSettings
    {
        public DatabaseSettings? Database { get; set; }

        public WordsSettings? Words { get; set; }

        public LearningSettings? Learning { get; set; }

        public PronunciationApiSettings? PronunciationApi { get; set; }
    }
}
