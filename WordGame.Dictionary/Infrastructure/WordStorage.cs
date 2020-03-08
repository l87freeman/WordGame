namespace WordGame.Dictionary.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helpers;
    using Interfaces;

    public class WordStorage : IWordStorage
    {
        private readonly Lazy<Dictionary<char, HashSet<string>>> wordStorage;

        public WordStorage(IWordProvider wordProvider)
        {
            ExceptionHelpers.ThrowOnNullArgument(nameof(wordProvider), wordProvider);
            this.wordStorage = new Lazy<Dictionary<char, HashSet<string>>>(wordProvider.GetWords);
        }

        public ISet<string> GetWords(char startLetter)
        {
            var result = new HashSet<string>();
            var storage = this.wordStorage.Value;
            startLetter = char.ToLower(startLetter);

            if (storage.ContainsKey(startLetter))
            {
                result = storage[startLetter];
            }

            return result;
        }

        public bool IsWordExists(string word)
        {
            var result = this.IsValidEntry(word) 
                         && this.GetWords(word[0]).Any(storedWord => 
                             string.Equals(storedWord, word, StringComparison.InvariantCultureIgnoreCase));
            return result;
        }

        private bool IsValidEntry(string word)
        {
            return !string.IsNullOrWhiteSpace(word) && word.Length > 2;
        }
    }
}