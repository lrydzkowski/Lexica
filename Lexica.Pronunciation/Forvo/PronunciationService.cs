using Lexica.Core.Extensions;
using Lexica.Pronunciation.Forvo.Config;
using Lexica.Pronunciation.Forvo.Model;
using NetCoreAudio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Lexica.Pronunciation.Forvo
{
    public class PronunciationService : IPronunciation
    {
        public PronunciationService(ForvoSettings forvoSettings, ILogger<IPronunciation> logger)
        {
            ForvoSettings = forvoSettings;
            if (!Directory.Exists(ForvoSettings.DownloadTempPath))
            {
                throw new Exception(
                    $"Directory {ForvoSettings.DownloadTempPath} from settings (Forvo.DownloadTempPath) doesn't exist."
                );
            }
            Logger = logger;
        }

        public ForvoSettings ForvoSettings { get; private set; }

        public ILogger<IPronunciation> Logger { get; private set; }

        public async Task<bool> PlayAsync(string word)
        {
            return await PlayAsync(new List<string>() { word });
        }

        public async Task<bool> PlayAsync(List<string> words)
        {
            var player = new Player();
            bool played = false;
            foreach (string word in words)
            {
                string responseContent = await SendRequest(word);
                List<RecordInfo> recordsInfo = ParseResponse(responseContent);
                if (recordsInfo.Count == 0)
                {
                    Logger.LogError($"{word} - No pronunciation");
                    continue;
                }
                var rnd = new Random();
                RecordInfo recordInfo = recordsInfo[rnd.Next(0, recordsInfo.Count - 1)];
                string mp3FilePath = await DownloadFile(recordInfo.PathMp3, word, recordInfo.Id);
                player.Play(mp3FilePath).Wait();
                played = true;
            }

            return played;
        }

        public async Task<string> SendRequest(string word)
        {
            using var client = new HttpClient();
            string url = GetApiUrl(word);
            HttpResponseMessage responseMsg = await client.GetAsync(url);
            HttpStatusCode statusCode = responseMsg.StatusCode;
            string content = await responseMsg.Content.ReadAsStringAsync();
            if (statusCode != HttpStatusCode.OK)
            {
                Logger.LogError(content);
                return "";
            }
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

        public List<RecordInfo> ParseResponse(string response)
        {
            var recordsInfo = new List<RecordInfo>();
            JsonDocument jsonDoc;
            try 
            {
                jsonDoc = JsonDocument.Parse(response); 
            } 
            catch 
            {
                return recordsInfo;
            }
            bool gettingItemsResult = jsonDoc.RootElement.TryGetProperty("items", out JsonElement items);
            if (!gettingItemsResult)
            {
                return recordsInfo;
            }
            foreach (JsonElement item in items.EnumerateArray())
            {
                bool gettingId = item.TryGetProperty("id", out JsonElement id);
                bool gettingPathMp3 = item.TryGetProperty("pathmp3", out JsonElement pathMp3);
                if (gettingId && gettingPathMp3)
                {
                    string? idVal = id.ToString();
                    string? pathMp3Val = pathMp3.ToString();
                    if (idVal != null && pathMp3Val != null)
                    {
                        recordsInfo.Add(new RecordInfo()
                        {
                            Id = idVal,
                            PathMp3 = pathMp3Val
                        });
                    }
                }
            }
            return recordsInfo;
        }

        public async Task<string> DownloadFile(
            string url,
            string word,
            string suffix = "",
            string extension = "mp3",
            bool overwrite = false)
        {
            using var webClient = new WebClient();
            string fileName = $"{word}-{suffix}".RemoveInvalidFileNameChars() + $".{extension}";
            string path = Path.Combine(ForvoSettings.DownloadTempPath, fileName);
            if (File.Exists(fileName) || overwrite)
            {
                return fileName;
            }
            await webClient.DownloadFileTaskAsync(new Uri(url), path);
            return path;
        }
    }
}
