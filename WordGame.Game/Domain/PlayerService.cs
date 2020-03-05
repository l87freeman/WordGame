namespace WordGame.Game.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Models;
    using Models.Players;

    public class PlayerService : IPlayerService
    {
        private readonly ILogger<PlayerService> logger;
        private readonly List<PlayerInfo> players = new List<PlayerInfo>();
        private readonly BotPlayerInfo botPlayer = new BotPlayerInfo();
        private SpinLock locker = new SpinLock();
        private bool botAdded = false;

        public PlayerService(ILogger<PlayerService> logger)
        {
            this.logger = logger;
        }

        public event EventHandler<PlayerEventData> PlayersChanged;

        public PlayerInfo CurrentPlayer { get; private set; }

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
                    this.Add(this.botPlayer);
                    this.botAdded = true;
                    Task.Delay(200).GetAwaiter().GetResult();
                    this.Invoke(this.botPlayer, PlayerEventType.PlayerJoined);
                }
                else if(this.botAdded)
                {
                    this.Remove(this.botPlayer);
                    this.botAdded = false;
                    this.Invoke(this.botPlayer, PlayerEventType.PlayerLeft);
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

        public void NextPlayer()
        {
            this.ExecuteWithSync(() =>
            {
                if (!this.TryShiftToNextPlayer(this.CurrentPlayer, out var nextPlayer))
                {
                    this.logger.LogError($"{this.CurrentPlayer} player was not found in players collection");
                    throw new InvalidOperationException(
                        $"Was not able to found a player {this.CurrentPlayer} in players collection");
                }

                this.CurrentPlayer = nextPlayer;
            });

            this.Invoke(this.CurrentPlayer, PlayerEventType.NextPlayer);
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