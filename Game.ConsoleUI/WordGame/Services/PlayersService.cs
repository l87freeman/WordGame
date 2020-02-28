namespace Game.ConsoleUI.WordGame.Services
{
    using Game.ConsoleUI.Interfaces.Services;
    using Game.ConsoleUI.Interfaces.Views;
    using Game.ConsoleUI.WordGame.Models;
    using Serilog;

    public class PlayersService : BaseServiceWithLogger<PlayersService>, IPlayersService
    {
        private readonly IBotService botService;
        private readonly IPlayersServiceView view;
        private readonly GameState gameState;

        public PlayersService(ILogger logger,
            IGameStateService gameStateService,
            IBotService botService,
            IPlayersServiceView view) : base(logger)
        {
            this.botService = botService;
            this.view = view;
            this.gameState = gameStateService.GetOrCreateGameState();
        }

        public bool ApproveResolution(GameChallenge challenge)
        {
            var agreed = true;
            var currentPlayer = this.gameState.CurrentPlayer;
            foreach (var player in this.gameState.Players)
            {
                if (player == currentPlayer || player is GamePlayerBot)
                {
                    continue;
                }

                if (!this.view.ApproveResolution(player.Name, challenge.CurrentSuggestedResolution, this.gameState.CurrentPlayer.Name))
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