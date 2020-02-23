namespace Game.ConsoleUI.Game.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Game.ConsoleUI.Interfaces.Services;
    using global::Game.ConsoleUI.Interfaces.Views;
    using Models;
    using Serilog;

    public class GameStateService : IGameStateService
    {
        private readonly ILogger logger;
        private readonly IBackupService backupService;
        private readonly IPlayerProvider playerProvider;
        private readonly IGameStateServiceView view;
        private GameState gameState;

        private bool isStateAfterRestored = false;

        public GameStateService(ILogger logger, IBackupService backupService, IPlayerProvider playerProvider, IGameStateServiceView view)
        {
            this.logger = logger.ForContext<GameStateService>();
            this.backupService = backupService;
            this.playerProvider = playerProvider;
            this.view = view;
        }

        public GameState GetOrCreateGameState()
        {
            if (this.gameState == null)
            {
                if (this.backupService.TryRestoreGame(out var gameStateRestored) 
                    && this.view.ShouldRestoreGame(gameStateRestored))
                {
                    this.gameState = gameStateRestored;
                    this.isStateAfterRestored = true;
                }
                else
                {
                    this.CreateNewState();
                }
            }
            
            return this.gameState;
        }

        public GamePlayer NextPlayer()
        {
            if (this.isStateAfterRestored)
            {
                this.isStateAfterRestored = false;
            }
            else
            {
                this.ShiftPlayers();
            }

            return this.gameState.CurrentPlayer;
        }

        private void ShiftPlayers()
        {
            var players = this.GetPlayers();
            if (this.gameState.CurrentPlayer == null)
            {
                this.gameState.CurrentPlayer = players.First();
            }
            else
            {
                var currentPlayer = players.First(player => player.Id == this.gameState.CurrentPlayer.Id);
                var currentIndex = players.IndexOf(currentPlayer);
                this.gameState.CurrentPlayer = ++currentIndex > players.Count -1 ? players[0] : players[currentIndex];
            }
        }

        public List<GamePlayer> GetPlayers()
        {
            return this.gameState.Players;
        }

        private void CreateNewState()
        {
            var newGameState = new GameState();
            var players = this.playerProvider.GetPlayers();
            this.AddPlayers(newGameState, players);

            this.gameState = newGameState;

            this.gameState.StateChanged += StoreStateChanged;
        }

        private void AddPlayers(GameState newGameState, List<GamePlayer> players)
        {
            foreach (var gamePlayer in players)
            {
                newGameState.Players.Add(gamePlayer);
                this.logger.Debug($"Player {gamePlayer} was added to game players");
            }
        }

        private void StoreStateChanged(object sender, System.EventArgs e)
        {
            if (!this.backupService.TryStoreGame(this.gameState))
            {
                this.logger.Warning($"Was not able to store game state {this.gameState}");
            }
        }
    }
}