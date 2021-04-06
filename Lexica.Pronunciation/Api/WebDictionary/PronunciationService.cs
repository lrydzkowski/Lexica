using Lexica.Core.Extensions;
using Lexica.Core.Services;
using Lexica.Pronunciation.Api.WebDictionary.Config;
using Microsoft.Extensions.Logging;
using NetCoreAudio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lexica.Pronunciation.Api.WebDictionary
{
    public class PronunciationService : IPronunciation
    {
        public PronunciationService(
            WebDictionarySettings settings, 
            ILogger<IPronunciation> logger,
            UrlService urlService)
        {
            Settings = settings;
            if (!Directory.Exists(settings.DownloadTempPath))
            {
                throw new Exception(
                    $"Directory {settings.DownloadTempPath} from settings (WebDictionary.DownloadTempPath) doesn't exist."
                );
            }
            Logger = logger;
            UrlService = urlService;
        }

        public WebDictionarySettings Settings { get; private set; }

        public ILogger<IPronunciation> Logger { get; private set; }

        public UrlService UrlService { get; private set; }

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
                string? mp3FilePath = GetFilePath(word);
                if (!File.Exists(mp3FilePath))
                {
                    string? mp3FileUrl = await GetFileUrl(word);
                    if (mp3FileUrl == null)
                    {
                        continue;
                    }
                    mp3FilePath = await DownloadFile(mp3FileUrl, word);
                    if (!File.Exists(mp3FilePath))
                    {
                        continue;
                    }
                }
                await player.Play(mp3FilePath);
                played = true;
            }

            return played;
        }

        public string GetFilePath(string word, string extension = "mp3")
        {
            string fileName = word.RemoveInvalidFileNameChars() + '.' + extension;
            return Path.Combine(Settings.DownloadTempPath, fileName);
        }

        public async Task<string?> GetFileUrl(string word)
        {
            using var client = new HttpClient();
            string dictionaryPageUrl = GetPageUrl(word);
            HttpResponseMessage responseMsg = await client.GetAsync(dictionaryPageUrl);
            HttpStatusCode statusCode = responseMsg.StatusCode;
            string content = await responseMsg.Content.ReadAsStringAsync();
            if (statusCode != HttpStatusCode.OK)
            {
                Logger.LogError(content);
                return null;
            }

            var rg = new Regex(@"/[^>=]+us_pron[^>]+\.mp3", RegexOptions.IgnoreCase);
            MatchCollection matches = rg.Matches(content);
            string? fileUrl = null;
            foreach (Match match in matches)
            {
                fileUrl = match.Value;
                break;
            }
            if (fileUrl == null)
            {
                return null;
            }
            return UrlService.Combine(Settings.Host, fileUrl);
        }

        public string GetPageUrl(string word)
        {
            string escapedWord = word.Replace(" ", "-");
            return UrlService.Combine(Settings.Host, Settings.UrlPath, escapedWord);
        }

        public async Task<string?> DownloadFile(
            string url,
            string word,
            string extension = "mp3",
            bool overwrite = false)
        {
            string filePath = GetFilePath(word, extension);
            if (File.Exists(filePath) && !overwrite)
            {
                return filePath;
            }
            using var webClient = new WebClient();
            await webClient.DownloadFileTaskAsync(new Uri(url), filePath);
            return filePath;
        }
    }
}
