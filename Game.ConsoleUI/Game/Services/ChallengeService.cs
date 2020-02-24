namespace Game.ConsoleUI.Game.Services
{
    using System.Linq;
    using global::Game.ConsoleUI.Interfaces.Services;
    using Infrastructure.Helpers;
    using Models;
    using Serilog;

    public class ChallengeService : BaseServiceWithLogger<ChallengeService>, IChallengeService
    {
        private readonly IValidWordVerifier validWordVerifier;
        private readonly GameState gameState;

        public ChallengeService(ILogger logger, IGameStateService gameStateService, IValidWordVerifier validWordVerifier) : base(logger)
        {
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

            private set => this.gameState.CurrentChallenge = value;
        }

        public void Resolved()
        {
            ExceptionHelpers.ThrowOnInvalidOperation("Valid resolution for challenge was not provided", this.IsResolutionValid);

            var resolution = this.Challenge.SuggestedResolution;
            this.Challenge.Resolve(resolution);
            var newChallengeLetter = this.Challenge.ChallengeResolution.Last();
            this.CreateChallenge(newChallengeLetter);
        }

        private void CreateChallenge(char letter)
        {
            var newChallenge = new GameChallenge(letter);
            
            this.gameState.Challenges.Add(newChallenge);
            this.Challenge = newChallenge;
        }

        public bool IsResolutionValid()
        {
            var resolution = this.Challenge.SuggestedResolution;
            var history = this.gameState.Challenges;
            var isValid = this.validWordVerifier.IsValid(history, resolution);

            return isValid;
        }
    }
}