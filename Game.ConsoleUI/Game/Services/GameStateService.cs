namespace Game.ConsoleUI.Game.Services
{
    using System.Collections.Generic;
    using global::Game.ConsoleUI.Interfaces.Services;
    using global::Game.ConsoleUI.Interfaces.Views;
    using Models;
    using Serilog;

    public class GameStateService : BaseServiceWithLogger<GameStateService>, IGameStateService
    {
        private readonly IBackupService backupService;
        private readonly IPlayerProvider playerProvider;
        private readonly IGameStateServiceView view;
        private GameState gameState;

        public GameStateService(ILogger logger, 
            IBackupService backupService,
            IPlayerProvider playerProvider,
            IGameStateServiceView view) : base(logger)
        {
            this.backupService = backupService;
            this.playerProvider = playerProvider;
            this.view = view;
        }

        public GameState GetOrCreateGameState()
        {
            if (this.gameState == null)
            {
                if (!this.TryRestoreGame(out var newGameState))
                {
                    newGameState = this.CreateNewState();
                }

                this.SetGameState(newGameState);
            }
            
            return this.gameState;
        }

        private bool TryRestoreGame(out GameState newGameState)
        {
            return this.backupService.TryRestoreGame(out newGameState)
                   && this.view.ShouldRestoreGame(newGameState);
        }

        private void SetGameState(GameState newGameState)
        {
            this.gameState = newGameState;
            this.gameState.StateChanged += this.StoreStateChanged;
        }

        private GameState CreateNewState()
        {
            var newGameState = new GameState();
            var players = this.playerProvider.GetPlayers();
            this.AddPlayers(newGameState, players);

            return newGameState;
        }

        private void AddPlayers(GameState newGameState, List<GamePlayer> players)
        {
            foreach (var gamePlayer in players)
            {
                newGameState.AddPlayer(gamePlayer);
                this.Logger.Debug($"Player {gamePlayer} was added to game players");
            }
        }

        private void StoreStateChanged(object sender, System.EventArgs e)
        {
            if (!this.backupService.TryStoreGame(this.gameState))
            {
                this.Logger.Warning($"Was not able to store game state {this.gameState}");
            }
        }
    }
}