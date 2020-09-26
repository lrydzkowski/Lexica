using Lexica.Core.Config;
using Lexica.Core.Config.Models;
using Lexica.Core.Exceptions;
using Lexica.Core.IO;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Xunit;

namespace CoreTests
{
    public class AppSettingsTests
    {
        public static IEnumerable<object[]> GetWrongConfigurationParameters()
        {
            return new List<object[]>
            {
                new object[]
                {
                    "Resources.appsettings1.wrong.json",
                    new Dictionary<string, string>() {
                        { "0", "Invalid type. Expected String but got Boolean. Path 'Database.ConnectionString', line 3, position 29." }
                    }
                },
                new object[]
                {
                    "Resources.appsettings2.wrong.json",
                    new Dictionary<string, string>() {
                        { "0", "Required properties are missing from object: PlayPronunciation. Path 'Maintaining', line 14, position 18." },
                        { "1", "Required properties are missing from object: Words. Path '', line 1, position 1." }
                    }
                },
                new object[]
                {
                    "Resources.appsettings3.wrong.json",
                    new Dictionary<string, string>() {
                        { "0", "Integer 11 exceeds maximum value of 10. Path 'Spelling.NumOfLevels', line 10, position 21." },
                        { "1", "Integer 11 exceeds maximum value of 10. Path 'Learning.NumOfLevels', line 14, position 21." }
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetWrongConfigurationParameters))]
        public void Init_WrongConfiguration_ThrowsWrongConfigException(
            string resourcePath, 
            Dictionary<string, string> expectedExceptionDetails)
        {
            // Arrange
            var resourceConfigSource = new EmbeddedSource(resourcePath, Assembly.GetExecutingAssembly());

            // Act
            void action() => new AppSettings(resourceConfigSource);

            // Assert
            WrongConfigException ex = Assert.Throws<WrongConfigException>(action);
            Assert.Equal("App settings has a wrong structure.", ex.Message);
            Assert.Equal(expectedExceptionDetails.Count, ex.Data.Count);
            foreach (KeyValuePair<string, string> el in expectedExceptionDetails)
            {
                Assert.True(ex.Data.Contains(el.Key));
                Assert.Equal(el.Value, ex.Data[el.Key]);
            }
        }

        public static IEnumerable<object[]> GetCorrectConfigurationParameters()
        {
            return new List<object[]>
            {
                new object[]
                {
                    "Resources.appsettings4.correct.json",
                    new Settings
                    {
                        Database = new Database
                        {
                            ConnectionString = "Host=my_host;Database=my_db;Username=my_user;Password=my_pw"
                        },
                        Words = new Words
                        {
                            ImportDirectoryPath = "c:\\LexicaImport"
                        },
                        Spelling = new Spelling
                        {
                            ForvoAPI = false,
                            NumOfLevels = 2,
                            ResetAfterMistake = true
                        },
                        Learning = new Learning
                        {
                            NumOfLevels = 2,
                            PlayPronunciation = false
                        },
                        Maintaining = new Maintaining
                        {
                            ResetAfterMistake = true,
                            PlayPronunciation = false
                        }
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetCorrectConfigurationParameters))]
        public void Get_CorrectConfiguration_ReturnsSettingsObject(
            string resourcePath,
            Settings expectedSettings)
        {
            // Arrange
            var resourceConfigSource = new EmbeddedSource(resourcePath, Assembly.GetExecutingAssembly());
            var appSettings = new AppSettings(resourceConfigSource);

            // Act
            Settings settings = appSettings.Get();

            // Assert
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            string expectedSettingsJson = JsonSerializer.Serialize(expectedSettings, options);
            string settingsJson = JsonSerializer.Serialize(settings, options);

            Assert.Equal(expectedSettingsJson, settingsJson);
        }
    }
}
