﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Pronunciation.Api.WebDictionary.Config
{
    public class WebDictionarySettings
    {
        public string Host { get; set; } = "";

        public string UrlPath { get; set; } = "";

        public string DownloadDirectoryPath { get; set; } = "";
    }
}
