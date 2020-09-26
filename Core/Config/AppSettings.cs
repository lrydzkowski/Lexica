using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lexica.Core.Config.Models;
using Lexica.Core.Exceptions;
using Lexica.Core.IO;
using Lexica.Core.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Lexica.Core.Config
{
    public class AppSettings
    {
        private string DefaultSchemaPath { get; } = "Resources.appsettings.schema.json";

        private ISource ConfigSource { get; set; }

        private ISource ConfigSchemaSource { get; set; }

        public Settings Settings { get; private set; }

        public AppSettings(ISource configSource, ISource configSchemaSource = null)
        {
            ConfigSource = configSource;
            ConfigSchemaSource = configSchemaSource ?? new EmbeddedSource(DefaultSchemaPath);
            LoadConfig();
        }

        private OperationResult<IList<string>> Validate(string configContents, string configSchemaContents)
        {
            JSchema schema = JSchema.Parse(configSchemaContents);
            JObject confObj = JObject.Parse(configContents);

            IList<string> validationErrors = null;
            bool result = confObj.IsValid(schema, out validationErrors);
            var operationResult = new OperationResult<IList<string>>(result, validationErrors);

            return operationResult;
        }

        private void LoadConfig()
        {
            string configContents = ConfigSource.GetContents();
            string configSchemaContents = ConfigSchemaSource.GetContents();
            OperationResult<IList<string>> validationResult = Validate(configContents, configSchemaContents);
            if (!validationResult.Result)
            {
                var exception = new WrongConfigException("App settings has a wrong structure.");
                for (int i = 0; i < validationResult.Data.Count; i++)
                {
                    exception.Data.Add(i.ToString(), validationResult.Data[i]);
                }
                throw exception;
            }
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(configContents)))
            {
                Settings settings = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build()
                    .Get<Settings>();
                Settings = settings;
            }
        }

        public Settings Get(bool reload = false)
        {
            if (reload)
            {
                LoadConfig();
            }
            return Settings;
        }
    }
}
