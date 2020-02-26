namespace GameTests
{
    using System;
    using System.IO;
    using Xunit;
    using FluentAssertions;
    using Game.ConsoleUI.Infrastructure;
    using GameTests.Helpers;
    using Moq;
    using Serilog;


    public class FileWordProviderTests
    {
        private readonly Mock<ILogger> loggerMock = MockHelpers.LoggerMock<FileWordProvider>();

        [Fact]
        public void FileExists_ShouldReturnWords()
        {
            var config = this.CreateConfig("words_alpha.txt", "Resources");
            var provider = new FileWordProvider(config, this.loggerMock.Object);

            var works = provider.GetWords();
            works.Should().NotBeEmpty();
        }

        [Fact]
        public void FileNotExists_ShouldNotReturnWords()
        {
            var config = this.CreateConfig("someFile.txt", "Resources");
            var provider = new FileWordProvider(config, this.loggerMock.Object);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var works = provider.GetWords();
            });
            
            this.loggerMock.Verify(m => m.Error(It.IsAny<FileNotFoundException>(), It.IsAny<string>()));
        }

        [Fact]
        public void PathIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var provider = new FileWordProvider(null, this.loggerMock.Object);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var provider = new FileWordProvider(new GameConfiguration(), null);
            });
        }

        private GameConfiguration CreateConfig(string dictionaryFile, string dictionaryFolder)
        {
            var config = new GameConfiguration { DictionaryFile = dictionaryFile, DictionaryFolder = dictionaryFolder };

            return config;
        }
    }
}
