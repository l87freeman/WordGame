namespace Game.ConsoleUI.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Game.ConsoleUI.Infrastructure.Helpers;
    using Game.ConsoleUI.Infrastructure.Interfaces;
    using Game.ConsoleUI.WordGame.Services;
    using Microsoft.Extensions.Options;
    using Serilog;

    public class FileWordProvider : BaseServiceWithLogger<FileWordProvider>, IWordProvider
    {
        private readonly Dictionary<char, HashSet<string>> wordStorage = new Dictionary<char, HashSet<string>>();
        private readonly Task initializationTask;

        public FileWordProvider(IOptions<GameConfiguration> config, ILogger logger) : base(logger)
        {
            ExceptionHelpers.ThrowOnNullArgument(nameof(config), config);

            var fileName = config.Value.DictionaryFile;
            var fileFolder = config.Value.DictionaryFolder;
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileFolder, fileName);
            this.initializationTask = this.InitializeStorageAsync(filePath);
        }

        private Task InitializeStorageAsync(string fileLocation)
        {
            return Task.Run(() => { this.ReadFromFile(fileLocation); });
        }

        private void ReadFromFile(string fileLocation)
        {
            try
            {
                FileHelpers.FileReaderBorrower(fileLocation, this.StoreWord);
            }
            catch (Exception exception)
            {
                this.Logger.Error(exception, "Error occured while trying to read dictionary");
                ExceptionHelpers.ThrowOnInvalidOperation("Was not able to read word dictionary");
            }
        }

        private void StoreWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return;
            }

            word = word.ToLower();
            var charName = word[0];

            var wordsOnChar = this.GetOrCreateWordsOnChar(charName);
            wordsOnChar.Add(word);
        }

        private HashSet<string> GetOrCreateWordsOnChar(char charName)
        {
            if (!this.wordStorage.TryGetValue(charName, out var wordsOnChar))
            {
                wordsOnChar = new HashSet<string>();
                this.wordStorage.Add(charName, wordsOnChar);
            }

            return wordsOnChar;
        }

        public Dictionary<char, HashSet<string>> GetWords()
        {
            this.initializationTask.GetAwaiter().GetResult();

            return this.wordStorage;
        }
    }
}