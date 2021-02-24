using Lexica.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        [InlineData("Resource.Config.appsettings0.wrong.json")]
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

        public static IEnumerable<object[]> GetMultipleFilesCorrectPathParameters()
        {
            return new List<object[]>
            {
                new object[]
                {
                    "Resources/Multiple/Files",
                    new List<string>()
                    {
                        "file1 contents",
                        "file2 contents"
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetMultipleFilesCorrectPathParameters))]
        public void GetMultipleFiles_CorrectPath_ReturnsProperContents(string dirPath, List<string> expectedContents)
        {
            // Arrange
            var filesSource = new MultipleFilesSource(dirPath);
            
            // Act
            List<string> list = filesSource.GetContents().Select(x => x.GetContents()).ToList<string>();

            // Assert
            Assert.Equal(expectedContents.Count, list.Count);
            Assert.Equal(expectedContents[0], list[0]);
            Assert.Equal(expectedContents[1], list[1]);
        }

        public static IEnumerable<object[]> GetMultipleFilesCorrectNames()
        {
            return new List<object[]>
            {
                new object[]
                {
                    "Resources/Multiple/Files",
                    new List<string>()
                    {
                        "file1.txt",
                        "file2.txt"
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetMultipleFilesCorrectNames))]
        public void GetMultipleFiles_CorrectPath_ReturnsProperNames(string dirPath, List<string> expectedNames)
        {
            // Arrange
            var filesSource = new MultipleFilesSource(dirPath);

            // Act
            List<string> list = filesSource.GetContents().Select(x => x.Name).ToList<string>();

            // Assert
            Assert.Equal(expectedNames.Count, list.Count);
            Assert.Equal(expectedNames[0], list[0]);
            Assert.Equal(expectedNames[1], list[1]);
        }

        public static IEnumerable<object[]> GetMultipleEmbeddedFilesCorrectPathParameters()
        {
            return new List<object[]>
            {
                new object[]
                {
                    "Lexica.CoreTests/Resources/Multiple/Embedded",
                    new List<string>()
                    {
                        "embedded file 1 contents",
                        "embedded file 2 contents"
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetMultipleEmbeddedFilesCorrectPathParameters))]
        public void GetMultipleEmbeddedFiles_CorrectPath_ReturnsProperContents(
            string dirPath, 
            List<string> expectedContents)
        {
            // Arrange
            var embeddedSource = new MultipleEmbeddedSource(
                dirPath, 
                Assembly.GetExecutingAssembly()
            );

            // Act
            List<string> list = embeddedSource.GetContents().Select(x => x.GetContents()).ToList<string>();

            // Assert
            Assert.Equal(expectedContents.Count, list.Count);
            Assert.Equal(expectedContents[0], list[0]);
            Assert.Equal(expectedContents[1], list[1]);
        }

        public static IEnumerable<object[]> GetMultipleEmbeddedFilesCorrectNames()
        {
            return new List<object[]>
            {
                new object[]
                {
                    "Lexica.CoreTests/Resources/Multiple/Embedded",
                    new List<string>()
                    {
                        "embedded_file1.txt",
                        "embedded_file2.txt"
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetMultipleEmbeddedFilesCorrectNames))]
        public void GetMultipleEmbeddedFiles_CorrectPath_ReturnsProperNames(
            string dirPath, 
            List<string> expectedNames)
        {
            // Arrange
            var embeddedSource = new MultipleEmbeddedSource(
                dirPath,
                Assembly.GetExecutingAssembly()
            );

            // Act
            List<string> list = embeddedSource.GetContents().Select(x => x.Name).ToList<string>();

            // Assert
            Assert.Equal(expectedNames.Count, list.Count);
            Assert.Equal(expectedNames[0], list[0]);
            Assert.Equal(expectedNames[1], list[1]);
        }
    }
}
