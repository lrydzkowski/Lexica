using Lexica.Learning.Config;
using Lexica.Pronunciation.Config;
using Lexica.Words.Config;

namespace Lexica.CLI.Core.Config
{
    public class AppSettings
    {
        public WordsSettings? Words { get; set; } = null;

        public LearningSettings Learning { get; set; } = new LearningSettings();

        public PronunciationApiSettings? PronunciationApi { get; set; } = null;
    }
}