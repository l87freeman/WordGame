namespace WordGame.Game.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using WordGame.Game.Controllers;
    using WordGame.Game.Domain.Models;

    public class GameManager : IGameManager
    {
        private readonly ILogger<GameManager> logger;
        private readonly IHubContext<GameHub> hubContext;
        private SpinLock locker = new SpinLock();
        private bool isLockTaken = false;
        private Game game;

        public GameManager(ILogger<GameManager> logger, IHubContext<GameHub> hubContext)
        {
            this.logger = logger;
            this.hubContext = hubContext;
        }

        public Task ApplyApprovalAsync(PlayerInfo player, bool isApproved)
        {
            return Task.CompletedTask;
        }

        public Task ApplyResolutionAsync(PlayerInfo player, string message)
        {
            return Task.CompletedTask;
        }

        public async Task ChangedBotInteractionAsync(PlayerInfo player)
        {
            await this.ExecuteWithLock(() =>
            {
                this.game.AddOrRemoveBot();
                return Task.CompletedTask;
            });
        }

        private async Task ExecuteWithLock(Func<Task> actionToExecute)
        {
            try
            {
                this.locker.Enter(ref this.isLockTaken);
                await actionToExecute();
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Error occured while performing action");
            }
            finally
            {
                this.locker.Exit();
            }
        }

        private async Task CreateChallenge()
        {
            var challenge = new Challenge {Letter = 'a', Suggestions = new List<Suggestion>()};
            await this.hubContext.Clients.All.SendAsync("Challenge", challenge);
        }
    }
}