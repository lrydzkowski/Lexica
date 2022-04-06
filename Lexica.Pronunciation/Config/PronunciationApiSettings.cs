using Lexica.Pronunciation.Api.WebDictionary.Config;

namespace Lexica.Pronunciation.Config
{
    public class PronunciationApiSettings
    {
        public WebDictionarySettings WebDictionary { get; set; } = new WebDictionarySettings();
    }
}