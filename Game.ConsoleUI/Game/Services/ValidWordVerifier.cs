namespace Game.ConsoleUI.Game.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using global::Game.ConsoleUI.Game.Models;
    using global::Game.ConsoleUI.Interfaces.Services;
    using Infrastructure.Interfaces;
    using Serilog;

    public class ValidWordVerifier : BaseServiceWithLogger<ValidWordVerifier>, IValidWordVerifier
    {
        private readonly IWordStorage wordStorage;
        private readonly Regex validationPattern = new Regex("^[a-z]{2,}$", RegexOptions.IgnoreCase);

        public ValidWordVerifier(ILogger logger, IWordStorage wordStorage) : base(logger)
        {
            this.wordStorage = wordStorage;
        }

        private bool IsExistsInDictionary(string resolution)
        {
            var words = this.wordStorage.GetWords(resolution[0]);
            var isWordFeasible = words.Any(word =>
                string.Equals(word, resolution, StringComparison.InvariantCultureIgnoreCase));
            return isWordFeasible;
        }

        private bool IsConformToPattern(string resolution)
        {
            var isValidWord = this.validationPattern.IsMatch(resolution);

            return isValidWord;
        }

        private bool IsNotExistsInHistory(List<GameChallenge> history, string resolution)
        {
            var wasSuggested = history.SelectMany(h => h.HistoryOfSuggestedResolutions).Any(suggestedResolution =>
                string.Equals(resolution, suggestedResolution, StringComparison.InvariantCultureIgnoreCase));

            return !wasSuggested;
        }

        public bool IsValid(List<GameChallenge> history, string resolution)
        {
            var isValidWord = this.IsConformToPattern(resolution) 
                              && this.IsNotExistsInHistory(history, resolution) 
                              && this.IsExistsInDictionary(resolution);

            return isValidWord;
        }
    }
}