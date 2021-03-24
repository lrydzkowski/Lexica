using Lexica.Pronunciation.Forvo.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Pronunciation.Forvo
{
    public class PronunciationService : IPronunciation
    {
        public PronunciationService(ForvoSettings forvoSettings)
        {
            ForvoSettings = forvoSettings;
        }

        public ForvoSettings ForvoSettings { get; private set; }

        public async Task PlayAsync(string word)
        {
            using var client = new HttpClient();
            string url = GetApiUrl(word);
            HttpResponseMessage responseMsg = await client.GetAsync(url);
            HttpStatusCode statusCode = responseMsg.StatusCode;
            string content = await responseMsg.Content.ReadAsStringAsync();

        }

        public string GetApiUrl(string word)
        {
            var url = "https://api.forvo.com/";
            url += "action/word-pronunciations/";
            url += "format/json/";
            url += $"word/{word}/";
            url += $"language/{ForvoSettings.Language}/";
            url += $"country/{ForvoSettings.Country}/";
            url += $"order/{ForvoSettings.Order}/";
            url += $"limit/{ForvoSettings.Limit}/";
            url += $"key/{ForvoSettings.Key}";
            return url;
        }
    }
}
