using Lexica.Pronunciation.Api.Forvo.Config;
using Lexica.Pronunciation.Api.WebDictionary.Config;

namespace Lexica.Pronunciation.Config
{
    public class PronunciationApiSettings
    {
        public ForvoSettings Forvo { get; set; } = new ForvoSettings();

        public WebDictionarySettings WebDictionary { get; set; } = new WebDictionarySettings();
    }
}
