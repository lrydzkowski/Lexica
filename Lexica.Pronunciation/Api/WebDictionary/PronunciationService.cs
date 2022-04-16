using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Lexica.Core.Extensions;
using Lexica.Core.Services;
using Lexica.Pronunciation.Api.WebDictionary.Config;
using Microsoft.Extensions.Logging;
using NAudio.Wave;

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
            Logger = logger;
            UrlService = urlService;
        }

        public WebDictionarySettings Settings { get; }

        public ILogger<IPronunciation> Logger { get; }

        public UrlService UrlService { get; }

        public async Task<bool> AudioExists(string word)
        {
            return await AudioExists(new List<string> { word });
        }

        public async Task<bool> AudioExists(List<string> words)
        {
            foreach (string word in words)
            {
                string? mp3FilePath = await DownloadFile(word);
                if (mp3FilePath == null)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task PlayAsync(string word)
        {
            await PlayAsync(new List<string>() { word });
        }

        public async Task PlayAsync(List<string> words)
        {
            foreach (string word in words)
            {
                string? mp3FilePath = await DownloadFile(word);
                if (mp3FilePath != null)
                {
                    await PlayMp3Async(mp3FilePath);
                }
            }
        }

        private static async Task PlayMp3Async(string mp3FilePath)
        {
            using var audioFile = new AudioFileReader(mp3FilePath);
            using var outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Play();
            int totalTime = Convert.ToInt32(Math.Floor(audioFile.TotalTime.TotalMilliseconds));
            await Task.Delay(totalTime);
        }

        private async Task<string?> DownloadFile(string word)
        {
            string filePath = GetFilePath(word);
            if (File.Exists(filePath))
            {
                return filePath;
            }
            string? mp3FileUrl = await GetFileUrl(word);
            if (mp3FileUrl == null)
            {
                return null;
            }
            using var webClient = new WebClient();
            await webClient.DownloadFileTaskAsync(new Uri(mp3FileUrl), filePath);
            if (filePath == null || !File.Exists(filePath))
            {
                return null;
            }
            return filePath;
        }

        private string GetFilePath(string word, string extension = "mp3")
        {
            string fileName = word.RemoveInvalidFileNameChars() + '.' + extension;
            return Path.Combine(Settings.DownloadDirectoryPath, fileName);
        }

        private async Task<string?> GetFileUrl(string word)
        {
            using var client = new HttpClient();
            string dictionaryPageUrl = GetPageUrl(word);
            HttpResponseMessage responseMsg = await client.GetAsync(dictionaryPageUrl);
            HttpStatusCode statusCode = responseMsg.StatusCode;
            string content = await responseMsg.Content.ReadAsStringAsync();
            if (IsUrlRedirected(word, responseMsg))
            {
                return null;
            }
            if (statusCode != HttpStatusCode.OK)
            {
                Logger.LogError(content);
                return null;
            }
            string? fileUrl = GetAudioFileUrlFromHtml(word, content);
            if (fileUrl == null)
            {
                return null;
            }
            return UrlService.Combine(Settings.Host, fileUrl);
        }

        private string GetPageUrl(string word)
        {
            return UrlService.Combine(Settings.Host, GetPageUrlPath(word));
        }

        private bool IsUrlRedirected(string word, HttpResponseMessage responseMsg)
        {
            return GetPageUrlPath(word) != responseMsg.RequestMessage?.RequestUri?.AbsolutePath;
        }

        private string GetPageUrlPath(string word)
        {
            string escapedWord = word.Replace(" ", "-");
            return Settings.UrlPath.Replace("{word}", escapedWord);
        }

        private static string? GetAudioFileUrlFromHtml(string word, string content)
        {
            string? fileUrl = null;
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(content);
            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"pos-header dpos-h\"]");
            foreach (HtmlNode node in nodes)
            {
                HtmlNode titleNode = node.SelectSingleNode(".//div[@class=\"di-title\"]/span/span");
                if (titleNode == null)
                {
                    continue;
                }
                string? title = titleNode.InnerText?.Trim();
                if (title != word)
                {
                    continue;
                }
                HtmlNode audioSourceNode = node.SelectSingleNode(".//span[@class=\"us dpron-i \"]//source[@type=\"audio/mpeg\"]");
                if (audioSourceNode == null)
                {
                    continue;
                }
                string src = audioSourceNode.GetAttributeValue<string>("src", def: "");
                if (string.IsNullOrEmpty(src))
                {
                    continue;
                }
                fileUrl = src;
            }

            return fileUrl;
        }
    }
}
