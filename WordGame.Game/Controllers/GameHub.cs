namespace WordGame.Game.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Domain.Interfaces;
    using Domain.Models;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;

    public class GameHub : Hub
    {
        private readonly ILogger<GameHub> logger;

        public GameHub(ILogger<GameHub> logger, IGameManager gameManager)
        {
            this.logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var player = this.GetPlayerInfo();
            this.logger.LogDebug($"Player {player} joined this game");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var player = this.GetPlayerInfo();
            this.logger.LogDebug($"Player {player} left this game");
            await base.OnDisconnectedAsync(exception);
        }

        private PlayerInfo GetPlayerInfo()
        {
            var context = this.Context.GetHttpContext();
            var connectionId = context.Connection.Id;
            if(!context.Request.Cookies.TryGetValue("userName", out var playerName))
            {
                playerName = "Jon Dow";
            }

            return new PlayerInfo { Connection = connectionId, Name = playerName };
        }
    }
}