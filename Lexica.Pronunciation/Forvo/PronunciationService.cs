using Lexica.Core.Extensions;
using Lexica.Pronunciation.Forvo.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
            await PlayAsync(new List<string>() { word });
        }

        public async Task PlayAsync(List<string> words)
        {
            foreach (string word in words)
            {
                string responseContent = await SendRequest(word);
                List<string> urls = ParseResponse(responseContent);
            }
        }

        public async Task<string> SendRequest(string word)
        {
            using var client = new HttpClient();
            string url = GetApiUrl(word);
            HttpResponseMessage responseMsg = await client.GetAsync(url);
            HttpStatusCode statusCode = responseMsg.StatusCode;
            if (statusCode != HttpStatusCode.OK)
            {
                return "";
            }
            string content = await responseMsg.Content.ReadAsStringAsync();
            return content;
        }

        public string GetApiUrl(string word)
        {
            var url = "https://apifree.forvo.com/";
            url += "action/word-pronunciations/";
            url += "format/json/";
            url += $"word/{word}/";
            url += $"language/{ForvoSettings.Language}/";
            url += $"country/{ForvoSettings.Country}/";
            url += $"order/{ForvoSettings.Order}/";
            url += $"limit/{ForvoSettings.Limit}/";
            url += $"key/{ForvoSettings.Key}/";
            return url;
        }

        public List<string> ParseResponse(string response)
        {
            var urls = new List<string>();
            JsonDocument jsonDoc;
            try 
            {
                jsonDoc = JsonDocument.Parse(response); 
            } 
            catch 
            {
                return urls;
            }
            bool gettingItemsResult = jsonDoc.RootElement.TryGetProperty("items", out JsonElement items);
            if (!gettingItemsResult)
            {
                return urls;
            }
            foreach (JsonElement item in items.EnumerateArray())
            {
                bool gettingPathMp3 = item.TryGetProperty("pathmp3", out JsonElement pathMp3);
                if (gettingPathMp3)
                {
                    urls.Add(pathMp3.ToString());
                }
            }
            return urls;
        }

        //public async Task<string> DownloadFile(string url)
        //{
        //    using var webClient = new WebClient();
        //    await webClient.DownloadFileTaskAsync(new Uri(url), );
        //}
    }
}
