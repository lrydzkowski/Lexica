using Lexica.Core.Config;
using Lexica.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoreTests
{
    public class AppSettingsTests
    {
        [Theory]
        [InlineData("Resources.appsettings1.json")]
        [InlineData("Resources.appsettings2.json")]
        public void Load_WrongConfiguration_ThrowsWrongConfigException(string resourcePath)
        {
            var resourceConfigSource = new ResourceConfigSource(resourcePath);
            //AppSettings appSettings = new AppSettings(resourceConfigSource);

            WrongConfigException ex = Assert.Throws<WrongConfigException>(() => new AppSettings(resourceConfigSource));
            Assert.Equal("App settings has a wrong structure.", ex.Message);
        }
    }
}
