using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Lexica.CLI.Core.Services
{
    internal class JsonService : IService
    {
        public JsonSerializerOptions GetJsonSerializerOptions(
            bool writeIndented = false,
            JsonNamingPolicy? namingPolicy = null)
        {
            var encoderSettings = new TextEncoderSettings();
            // Additional options are needed to correct serialization of polish diacritic letters.
            encoderSettings.AllowRange(UnicodeRanges.BasicLatin);
            encoderSettings.AllowRange(UnicodeRanges.Latin1Supplement);
            encoderSettings.AllowRange(UnicodeRanges.LatinExtendedA);
            var options = new JsonSerializerOptions()
            {
                WriteIndented = writeIndented,
                Encoder = JavaScriptEncoder.Create(encoderSettings)
            };
            if (namingPolicy != null)
            {
                options.PropertyNamingPolicy = namingPolicy;
            }

            return options;
        }
    }
}