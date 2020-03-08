namespace WordGame.Game.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Models.Players;

    public class PlayerService : IPlayerService
    {
        private readonly ILogger<PlayerService> logger;
        private readonly List<Player> players = new List<Player>();
        private readonly BotPlayer botPlayer = new BotPlayer();

        public PlayerService(ILogger<PlayerService> logger)
        {
            this.logger = logger;
        }

        public Player CurrentPlayer { get; private set; }

        public bool IsGameWithBot { get; private set; }

        public List<Player> ActivePlayers => this.players.Where(p => p.IsActive).ToList();

        public List<Player> ActivePlayersNoCurrent => this.ActivePlayers.Where(p => p != this.CurrentPlayer).ToList();

        public Player GetPlayer(string playerId)
        {
            var player = this.players.FirstOrDefault(p => p.Id == playerId);

            return player;
        }

        public void Add(Player player)
        {
            this.ExecuteWithSync(() =>
            {
                this.players.Add(player);
            });
            
            this.logger.LogDebug($"Player [{player.Id} {player.Name}] was added to players collection");
            this.VerifyIfBotNeeded();
        }

        private void VerifyIfBotNeeded()
        {
            this.ExecuteWithSync(() =>
            {
                if (this.players.Count == 1 && !this.IsGameWithBot)
                {
                    this.Add(this.botPlayer);
                    this.IsGameWithBot = true;
                    Task.Delay(200).GetAwaiter().GetResult();
                }
                else if(this.IsGameWithBot)
                {
                    this.Remove(this.botPlayer);
                    this.IsGameWithBot = false;
                }
            });
        }

        public void Remove(Player player)
        {
            this.ExecuteWithSync(() =>
            {
                if (!this.players.Remove(player))
                {
                    this.logger.LogWarning($"Was not able to remove [{player.Id} {player.Name}] from players");
                }
                else
                {
                    this.logger.LogDebug($"[{player.Id} {player.Name}] was removed from players collection");
                }
            });
        }

        public void NextPlayer()
        {
            this.ExecuteWithSync(() =>
            {
                if (!this.TryShiftToNextPlayer(this.CurrentPlayer, out var nextPlayer))
                {
                    this.logger.LogError($"[{this.CurrentPlayer.Id} {this.CurrentPlayer.Name}] player was not found in players collection");
                    throw new InvalidOperationException(
                        $"Was not able to found a player [{this.CurrentPlayer.Id} {this.CurrentPlayer.Name}] in players collection");
                }

                this.CurrentPlayer = nextPlayer;
            });
        }

        public void Reset()
        {
            this.ExecuteWithSync(() =>
            {
                this.CurrentPlayer = null;
                this.players.Clear();
                this.IsGameWithBot = false;
            });
        }

        private bool TryShiftToNextPlayer(Player currentPlayer, out Player nextPlayer)
        {
            var nextPlayerSet = false;
            nextPlayer = null;
            var currentPlayerIndex = this.players.IndexOf(currentPlayer);

            if (this.ActivePlayers.Count > 0)
            {
                var nextPlayerIndex = ++currentPlayerIndex == this.players.Count ? 0 : currentPlayerIndex;
                nextPlayer = this.players[nextPlayerIndex];
                nextPlayerSet = true;
            }

            return nextPlayerSet;
        }

        private void ExecuteWithSync(Action actionToExecute)
        {
            try
            {
                lock (this)
                {
                    actionToExecute();
                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Error occured while performing action with lock");
                throw;
            }
        }
    }
}