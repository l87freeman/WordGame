namespace Game.ConsoleUI.WordGame
{
    using Game.ConsoleUI.Interfaces;
    using Game.ConsoleUI.Interfaces.Services;
    using Game.ConsoleUI.Interfaces.Views;
    using Game.ConsoleUI.WordGame.Models;
    using Game.ConsoleUI.WordGame.Services;
    using Serilog;

    public class GameManager : BaseServiceWithLogger<GameManager>, IGameManager
    {
        private readonly IPlayersService playersService;
        private readonly IValidWordVerifier challengeVerifier;
        private readonly IGameManagerView gameView;
        private readonly GameState gameState;

        public GameManager(ILogger logger,
            IPlayersService playersService,
            IGameStateService gameStateService,
            IValidWordVerifier challengeVerifier,
            IGameManagerView gameView) : base(logger)
        {
            this.playersService = playersService;
            this.challengeVerifier = challengeVerifier;
            this.gameState = gameStateService.GetOrCreateGameState();
            this.gameView = gameView;
        }

        public void NextTurn()
        {
            this.StartNewTurn();
            if (this.gameState.IsChallengeResolved)
            {
                this.gameState.NextChallenge();
            }
            this.PlayerResolveChallenge();
            this.gameState.ResolveChallenge();
        }

        private void StartNewTurn()
        {
            this.Logger.Debug("Next turn started");
            this.gameView.Refresh(this.gameState);
        }

        private void PlayerResolveChallenge()
        {
            do
            {
                this.playersService.ResolveChallenge(this.gameState.CurrentChallenge);
            } while (!this.IsResolutionValid() || !this.ApprovedByPlayers());
        }

        private bool IsResolutionValid()
        {
            var isValid = this.challengeVerifier.IsValid(this.gameState.ChallengeHistory,
                this.gameState.CurrentChallenge.CurrentSuggestedResolution);

            return isValid;
        }

        private bool ApprovedByPlayers()
        {
            var approved = this.playersService.ApproveResolution(this.gameState.CurrentChallenge);

            return approved;
        }
    }
}