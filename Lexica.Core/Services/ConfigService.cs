using Lexica.Core.Exceptions;
using Lexica.Core.IO;
using Lexica.Core.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Lexica.Core.Services
{
    public class ConfigService<T> where T : class
    {
        private ISource ConfigSource { get; set; }

        private ISource ConfigSchemaSource { get; set; }

        public T Config { get; private set; }

        public ConfigService(ISource configSource, ISource configSchemaSource)
        {
            ConfigSource = configSource;
            ConfigSchemaSource = configSchemaSource;
            Config = LoadConfig();
        }

        private static OperationResult<IList<string>> Validate(string configContents, string configSchemaContents)
        {
            JSchema schema = JSchema.Parse(configSchemaContents);
            JObject confObj = JObject.Parse(configContents);

            bool result = confObj.IsValid(schema, out IList<string> validationErrors);
            var operationResult = new OperationResult<IList<string>>(result, validationErrors);

            return operationResult;
        }

        private T LoadConfig()
        {
            string configContents = ConfigSource.GetContents();
            string configSchemaContents = ConfigSchemaSource.GetContents();
            OperationResult<IList<string>> validationResult = ConfigService<T>.Validate(
                configContents, configSchemaContents
            );
            if (!validationResult.Result)
            {
                var exception = new WrongConfigException($"File {ConfigSource.Name} has a wrong structure.");
                for (int i = 0; i < validationResult.Data.Count; i++)
                {
                    exception.Data.Add(i.ToString(), validationResult.Data[i]);
                }
                throw exception;
            }
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(configContents));
            T data = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build()
                .Get<T>();
            Config = data;

            return Config;
        }

        public T Get(bool reload = false)
        {
            if (reload)
            {
                return LoadConfig();
            }
            return Config;
        }

        public static ConfigService<T> Get(string name, Assembly assembly)
        {
            var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{name}.json");
            var configSource = new FileSource(configFilePath);
            var configSchemaSource = new EmbeddedSource($"{name}.schema.json", assembly);
            var configService = new ConfigService<T>(configSource, configSchemaSource);

            return configService;
        }
    }
}