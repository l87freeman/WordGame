namespace Game.ConsoleUI.Game.Services
{
    using System.Linq;
    using global::Game.ConsoleUI.Interfaces.Services;
    using global::Game.ConsoleUI.Interfaces.Views;
    using Models;
    using Serilog;

    public class PlayersService : IPlayersService
    {
        private readonly IGameStateService gameStateService;
        private readonly IBotService botService;
        private readonly IPlayersServiceView view;
        private readonly GameState gameState;
        private readonly ILogger logger;

        public PlayersService(ILogger logger, 
            IGameStateService gameStateService,
            IBotService botService,
            IPlayersServiceView view)
        {
            this.gameStateService = gameStateService;
            this.botService = botService;
            this.view = view;
            this.logger = logger.ForContext<PlayersService>();
            this.gameState = gameStateService.GetOrCreateGameState();
        }
        
        public void Shift()
        {
            this.gameStateService.NextPlayer();
        }

        public bool ApproveResolution(GameChallenge challenge)
        {
            var agreed = true;
            var currentPlayer = this.gameState.CurrentPlayer;
            foreach (var player in this.gameStateService.GetPlayers())
            {
                if (player == currentPlayer || player is GamePlayerBot)
                {
                    continue;
                }

                if (!this.view.ApproveResolution(player.Name, challenge.SuggestedResolution, this.gameState.CurrentPlayer.Name))
                {
                    agreed = false;
                    break;
                }
            }

            return agreed;
        }

        public void ResolveChallenge(GameChallenge challenge)
        {
            var currentPlayer = this.gameState.CurrentPlayer;
            string resolution = currentPlayer is GamePlayerBot ? 
                this.botService.ResolveChallenge(challenge) : 
                this.view.ResolveChallenge(currentPlayer.Name, challenge.ChallengeLetter);

            challenge.Suggest(resolution);
        }
    }
}