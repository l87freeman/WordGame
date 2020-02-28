namespace WordGame.Game.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using WordGame.Game.Domain.Interfaces;

    public class PlayersHub : Hub
    {
        private readonly ILogger<PlayersHub> logger;
        private readonly IGameManager gameManager;

        public PlayersHub(ILogger<PlayersHub> logger, IGameManager gameManager)
        {
            this.logger = logger;
            this.gameManager = gameManager;
        }

        public async Task StartGame()
        {
            
        }

        public override async Task OnConnectedAsync()
        {
            var user = this.GetUser();

            await this.Clients.Others.SendAsync("Notify", $"{user} joined this game");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = this.GetUser();

            await this.Clients.Others.SendAsync("Notify", $"{user} left this game");
            await base.OnDisconnectedAsync(exception);
        }

        private string GetUser()
        {
            if (this.TryGetUserName(out var userName))
            {
                //this.gameManager.AddPlayer(userName);
            }
            else
            {
                userName = Guid.NewGuid().ToString();
                this.Context.GetHttpContext().Response.Cookies.Append("userName", userName);

                this.logger.LogError($"User name for {this.Context.ConnectionId} was not provided, created new name {userName}");
            }

            return userName;
        }

        private bool TryGetUserName(out string userName)
        {
            var context = this.Context.GetHttpContext();
            var isUserNameFound = context.Request.Cookies.TryGetValue("userName", out userName);
                
            return isUserNameFound;
        }
    }
}