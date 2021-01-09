using Lexica.Core.Config;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lexica.Words.Config
{
    class Settings
    {
        public static AppSettings<Config.Models.Words> Get()
        {
            return AppSettings<Config.Models.Words>.GetSettings("database", Assembly.GetExecutingAssembly());
        }
    }
}
