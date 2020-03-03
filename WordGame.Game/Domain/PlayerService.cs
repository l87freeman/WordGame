namespace WordGame.Game.Domain
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Models;

    public class PlayerService : IPlayerService
    {
        private readonly ILogger<PlayerService> logger;
        private readonly ConcurrentDictionary<string, PlayerInfo> players = new ConcurrentDictionary<string, PlayerInfo>();
        private readonly BotPlayerInfo botInfo = new BotPlayerInfo();
        private SpinLock locker = new SpinLock();
        private bool botAdded = false;

        public PlayerService(ILogger<PlayerService> logger)
        {
            this.logger = logger;
        }

        public event EventHandler<string> PlayersChanged;

        public void Add(PlayerInfo player)
        {
            players[player.Connection] = player;
            var message = $"New player {player} joined this game";
            this.logger.LogDebug(message);
            this.PlayersChanged?.Invoke(this, message);

            this.VerifyIfBotNeeded();
        }

        private void VerifyIfBotNeeded()
        {
            this.ExecuteWithSync(() =>
            {
                string message = null;
                if (players.Count == 1 && !this.botAdded)
                {
                    this.players.TryAdd(this.botInfo.Connection, botInfo);
                    this.botAdded = true;
                    message = $"Bot {botInfo} was added to game";
                    
                }
                else if(this.botAdded)
                {
                    this.players.TryRemove(this.botInfo.Connection, out var botInfoRemoved);
                    this.botAdded = false;
                    message = $"Bot {botInfo} was removed from game";
                }

                if (message != null)
                {
                    this.PlayersChanged?.Invoke(this, message);
                    this.logger.LogDebug(message);
                }
            });
        }

        public void Remove(PlayerInfo player)
        {
            var message = $"player {player} left this game";
            if (!this.players.TryRemove(player.Connection, out var removePlayer))
            {
                this.logger.LogWarning($"Was not able to remove player {player} from players");
            }

            this.logger.LogDebug(message);
            this.PlayersChanged?.Invoke(this, message);
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