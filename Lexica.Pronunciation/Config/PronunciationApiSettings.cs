using Lexica.Pronunciation.Api.WebDictionary.Config;

namespace Lexica.Pronunciation.Config
{
    public class PronunciationApiSettings
    {
        public static string SectionName => "PronunciationApi";

        public WebDictionarySettings WebDictionary { get; set; } = new WebDictionarySettings();
    }
}
