namespace WordGame.Game.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Domain.Interfaces;
    using Domain.Models;
    using Domain.Models.Players;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;

    public class GameHub : Hub
    {
        private readonly ILogger<GameHub> logger;
        private readonly IGameManager gameManager;

        public GameHub(ILogger<GameHub> logger, IGameManager gameManager)
        {
            this.logger = logger;
            this.gameManager = gameManager;
        }

        public Task Approved(bool isApproved)
        {
            var player = this.GetPlayerInfo();
            this.gameManager.ApplyApproval(player, isApproved);
            return Task.CompletedTask;
        }

        public Task Resolved(string message)
        {
            var player = this.GetPlayerInfo();
            this.gameManager.ApplyResolution(player, message);
            return Task.CompletedTask;
        }

        public override async Task OnConnectedAsync()
        {
            var player = this.GetPlayerInfo();
            this.logger.LogDebug($"Player {player} joined this game");
            await base.OnConnectedAsync();
            this.gameManager.PlayerJoined(player);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var player = this.GetPlayerInfo();
            this.logger.LogDebug($"Player {player} left this game");
            await base.OnDisconnectedAsync(exception);
            this.gameManager.PlayerLeft(player);
        }

        private PlayerInfo GetPlayerInfo()
        {
            var context = this.Context.GetHttpContext();
            var connectionId = context.Connection.Id;
            if(!context.Request.Cookies.TryGetValue("userName", out var playerName))
            {
                this.logger.LogError($"For connection {connectionId} from {context.Connection.RemoteIpAddress} player name was not set");
                playerName = "Jon Dow";
            }

            return new PlayerInfo(connectionId, playerName);
        }
    }
}