namespace Game.ConsoleUI.Game.Services
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using global::Game.ConsoleUI.Interfaces.Services;
    using Infrastructure.Helpers;
    using Models;
    using Serilog;

    public class ChallengeService : IChallengeService
    {
        private readonly IValidWordVerifier validWordVerifier;
        private readonly ILogger logger;
        private readonly Regex validationPattern = new Regex("^[a-z]{2,}$", RegexOptions.IgnoreCase);
        private readonly GameState gameState;

        public ChallengeService(ILogger logger, IGameStateService gameStateService, IValidWordVerifier validWordVerifier)
        {
            this.logger = logger.ForContext<ChallengeService>();
            this.gameState = gameStateService.GetOrCreateGameState();
            this.validWordVerifier = validWordVerifier;
        }

        public GameChallenge Challenge
        {
            get
            {
                if (this.gameState.CurrentChallenge == null)
                {
                    this.CreateChallenge('a');
                }

                return this.gameState.CurrentChallenge;
            }
        }

        public void Resolved()
        {
            ExceptionHelpers.ThrowOnInvalidOperation("Valid resolution for challenge was not provided", this.IsResolutionValid);

            var resolution = this.gameState.CurrentChallenge.SuggestedResolution;
            this.gameState.CurrentChallenge.Resolve(resolution);
            var newChallengeLetter = this.gameState.CurrentChallenge.ChallengeResolution.Last();
            this.CreateChallenge(newChallengeLetter);
        }

        private void CreateChallenge(char letter)
        {
            var newChallenge = new GameChallenge(letter);
            
            this.gameState.Challenges.Add(newChallenge);
            this.gameState.CurrentChallenge = newChallenge;
        }

        public bool IsResolutionValid()
        {
            var resolution = this.gameState.CurrentChallenge.SuggestedResolution;
            var isValid = validationPattern.IsMatch(resolution)
                          && this.gameState.Challenges.Where(ch => ch.ChallengeResolution != null).All(ch => 
                              !string.Equals(resolution, ch.ChallengeResolution, StringComparison.InvariantCultureIgnoreCase))
                          && this.validWordVerifier.IsValid(resolution);

            return isValid;
        }
    }
}