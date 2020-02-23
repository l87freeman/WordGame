namespace Game.ConsoleUI.Game
{
    using global::Game.ConsoleUI.Interfaces;
    using global::Game.ConsoleUI.Interfaces.Services;
    using Interfaces.Views;
    using Models;
    using Serilog;

    public class GameManager : IGameManager
    {
        private readonly IPlayersService playersService;
        private readonly IChallengeService challengeService;
        private readonly IGameManagerView gameView;
        private readonly ILogger logger;
        private readonly GameState gameState;

        public GameManager(ILogger logger, 
            IPlayersService playersService, 
            IChallengeService challengeService,
            IGameStateService gameStateService,
            IGameManagerView gameView)
        {
            this.logger = logger.ForContext<GameManager>();
            this.playersService = playersService;
            this.challengeService = challengeService;
            this.gameState = gameStateService.GetOrCreateGameState();
            this.gameView = gameView;
        }

        public void NextTurn()
        {
            this.logger.Debug("Next turn started");
            this.gameView.Refresh(this.gameState);
            this.playersService.Shift();
            this.ResolveChallenge();
        }

        private void ResolveChallenge()
        {
            do
            {
                this.playersService.ResolveChallenge(this.challengeService.Challenge);
            } while (!this.challengeService.IsResolutionValid() 
                     || !this.playersService.ApproveResolution(this.challengeService.Challenge));

            this.challengeService.Resolved();
        }
    }
}