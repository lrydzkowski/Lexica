using Lexica.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Exceptions;
using System.Text;
using Xunit;

namespace CoreTests
{
    public class SourceTests
    {
        [Theory]
        [InlineData("appsettings2.json")]
        public void Init_NotExistedFile_ThrowsFileNotFoundException(string filePath)
        {
            // Act
            void action() => new FileSource(filePath);

            // Assert
            FileNotFoundException ex = Assert.Throws<FileNotFoundException>(action);
            Assert.Equal(String.Format("File {0} doesn't exist.", filePath), ex.Message);
        }

        [Theory]
        [InlineData("Resource.appsettings0.wrong.json")]
        public void Init_NotExistedEmbeddedResource_ThrowsFileNotFoundException(string resourcePath)
        {
            // Act
            void action()
            {
                var embeddedSource = new EmbeddedSource(resourcePath, Assembly.GetExecutingAssembly());
                embeddedSource.GetContents();
            }

            // Assert
            ResourceNotFoundException ex = Assert.Throws<ResourceNotFoundException>(action);
            Assert.Equal(String.Format("Resource ending with {0} not found.", resourcePath), ex.Message);
        }
    }
}
