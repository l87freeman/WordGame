namespace Game.ConsoleUI.WordGame.Services
{
    using System;
    using System.Linq;
    using Game.ConsoleUI.Infrastructure.Interfaces;
    using Game.ConsoleUI.Interfaces.Services;
    using Game.ConsoleUI.WordGame.Models;
    using Serilog;

    public class BotService : BaseServiceWithLogger<BotService>, IBotService
    {
        private readonly IWordStorage wordStorage;
        private readonly GameState gameState;

        private readonly Random random = new Random();

        public BotService(ILogger logger, IWordStorage wordStorage, IGameStateService stateService) : base(logger)
        {
            this.wordStorage = wordStorage;
            this.gameState = stateService.GetOrCreateGameState();
        }

        public string ResolveChallenge(GameChallenge challenge)
        {
            var letter = challenge.ChallengeLetter;
            var words = this.wordStorage.GetWords(letter);
            var usedWords = this.gameState.ChallengeHistory.Select(ch => ch.ChallengeResolution);
            var suggestedWords = this.gameState.ChallengeHistory.SelectMany(ch => ch.HistoryOfSuggestedResolutions);

            var feasibleWords = words
                .Where(word => usedWords.All(usedWord => !string.Equals(usedWord, word, StringComparison.InvariantCultureIgnoreCase)))
                .Where(word => suggestedWords.All(suggestedWord => !string.Equals(suggestedWord, word, StringComparison.InvariantCultureIgnoreCase)))
                .ToList();

            var wordIndex = this.random.Next(0, feasibleWords.Count - 1);

            return feasibleWords[wordIndex];
        }
    }
}