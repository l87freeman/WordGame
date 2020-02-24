namespace Game.ConsoleUI.Game.Services
{
    using System.Collections.Generic;
    using System.Linq;
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

        private bool isStateAfterRestored = false;

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
                if (this.backupService.TryRestoreGame(out var newGameState) 
                    && this.view.ShouldRestoreGame(newGameState))
                {
                    
                    this.isStateAfterRestored = true;
                }
                else
                {
                    newGameState = this.CreateNewState();
                }

                this.SetGameState(newGameState);
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
                newGameState.Players.Add(gamePlayer);
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