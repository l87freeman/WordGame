namespace GameTests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Game.ConsoleUI.Infrastructure;
    using Game.ConsoleUI.Infrastructure.Interfaces;
    using Moq;
    using Xunit;

    public class WordStorageTests
    {
        [Fact]
        public void GetWords_ShouldReturnEmptyListOnLetter()
        {
            var provider = this.CreateProviderMock();
            var storage = new WordStorage(provider);
            var stored = storage.GetWords('b');

            stored.Should().BeEmpty();
        }

        [Fact]
        public void GetWords_ShouldReturnEmptyListEmptyWords()
        {
            var words = new HashSet<string>();
            var provider = this.CreateProviderMock(('b', words));
            var storage = new WordStorage(provider);
            var stored = storage.GetWords('b');

            stored.Should().BeEmpty();
        }

        [Fact]
        public void GetWords_ShouldSearchByLower()
        {
            var words = new HashSet<string> { "beacon", "borsch" };
            var provider = this.CreateProviderMock(('b', words));
            var storage = new WordStorage(provider);
            var stored = storage.GetWords('B');

            stored.Should().HaveCount(words.Count);
        }

        private IWordProvider CreateProviderMock(params (char, HashSet<string>)[] wordsEntries)
        {
            wordsEntries = wordsEntries ?? new[] { ('a', new HashSet<string> { "abc", "acd", "acc" }) };

            var wordsMock = wordsEntries.ToDictionary(entry => entry.Item1, entry => entry.Item2);
            var providerMock = new Mock<IWordProvider>();
            providerMock.Setup(m => m.GetWords()).Returns(wordsMock);

            return providerMock.Object;
        }
    }
}