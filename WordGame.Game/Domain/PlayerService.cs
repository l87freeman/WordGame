namespace WordGame.Game.Domain
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Models;

    public class PlayerService : IPlayerService
    {
        private readonly ILogger<PlayerService> logger;
        private readonly List<PlayerInfo> players = new List<PlayerInfo>();
        private readonly BotPlayerInfo botInfo = new BotPlayerInfo();
        private SpinLock locker = new SpinLock();
        private bool botAdded = false;

        public PlayerService(ILogger<PlayerService> logger)
        {
            this.logger = logger;
        }

        public event EventHandler<PlayerEventData> PlayersChanged;

        public void Add(PlayerInfo player)
        {
            this.ExecuteWithSync(() =>
            {
                this.players.Add(player);
            });
            
            this.logger.LogDebug($"{player} was added to players collection");
            this.Invoke(player, PlayerEventType.PlayerJoined);

            this.VerifyIfBotNeeded();
        }

        private void Invoke(PlayerInfo player, PlayerEventType eventType)
        {
            var eventData = new PlayerEventData
                {EventByPlayer = player, EventType = eventType, PlayersCount = this.players.Count};

            this.PlayersChanged?.Invoke(this, eventData);
        }

        private void VerifyIfBotNeeded()
        {
            this.ExecuteWithSync(() =>
            {
                if (players.Count == 1 && !this.botAdded)
                {
                    this.Add(this.botInfo);
                    this.botAdded = true;
                }
                else if(this.botAdded)
                {
                    this.Remove(this.botInfo);
                    this.botAdded = false;
                }
            });
        }

        public void Remove(PlayerInfo player)
        {
            this.ExecuteWithSync(() =>
            {
                if (!this.players.Remove(player))
                {
                    this.logger.LogWarning($"Was not able to remove player {player} from players");
                }
                else
                {
                    this.logger.LogDebug($"{player} was removed from players collection");
                }
            });
            
            this.Invoke(player, PlayerEventType.PlayerLeft);
        }

        public PlayerInfo NextPlayer(PlayerInfo currentPlayer)
        {
            PlayerInfo nextPlayer = null;
            this.ExecuteWithSync(() =>
            {
                if (!this.TryShiftToNextPlayer(currentPlayer, out nextPlayer))
                {
                    this.logger.LogError($"{currentPlayer} player was not found in players collection");
                    throw new InvalidOperationException(
                        $"Was not able to found a player {currentPlayer} in players collection");
                }
            });

            this.Invoke(nextPlayer, PlayerEventType.NextPlayer);
            return nextPlayer;
        }

        private bool TryShiftToNextPlayer(PlayerInfo currentPlayer, out PlayerInfo nextPlayer)
        {
            nextPlayer = null;
            var currentPlayerIndex = this.players.IndexOf(currentPlayer);

            if (currentPlayerIndex != -1 && this.players.Count > 0)
            {
                var nextPlayerIndex = ++currentPlayerIndex == this.players.Count ? 0 : currentPlayerIndex;
                nextPlayer = this.players[nextPlayerIndex];
            }

            return currentPlayer != nextPlayer;
        }

        private void ExecuteWithSync(Action actionToExecute)
        {
            var lockTaken = false;
            try
            {
                this.locker.Enter(ref lockTaken);
                actionToExecute();
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Error occured while performing action");
            }
            finally
            {
                if (lockTaken)
                {
                    this.locker.Exit(false);
                }
            }
        }
    }
}