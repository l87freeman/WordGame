namespace GameTests
{
    using System;
    using Xunit;
    using FluentAssertions;
    using Game.ConsoleUI.Infrastructure;


    public class FileWordProviderTests
    {
        [Fact]
        public void FileExists_ShouldReturnWords()
        {
            var config = this.CreateConfig("words_alpha.txt", "Resources");
            var provider = new FileWordProvider(config);

            var works = provider.GetWords();
            works.Should().NotBeEmpty();
        }

        [Fact]
        public void FileNotExists_ShouldNotReturnWords()
        {
            var config = this.CreateConfig("someFile.txt", "Resources");
            var provider = new FileWordProvider(config);

            var works = provider.GetWords();
            works.Should().BeEmpty();
        }

        [Fact]
        public void PathIsNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var provider = new FileWordProvider(null);
            });
        }

        private GameConfiguration CreateConfig(string dictionaryFile, string dictionaryFolder)
        {
            var config = new GameConfiguration { DictionaryFile = dictionaryFile, DictionaryFolder = dictionaryFolder };

            return config;
        }
    }
}
