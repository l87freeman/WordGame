namespace WordGame.Game.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Domain.Interfaces;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;

    public class GameHub : Hub
    {
        private readonly ILogger<GameHub> logger;
        private readonly ICommunicationProxy communicationProxy;

        public GameHub(ILogger<GameHub> logger, ICommunicationProxy communicationProxy)
        {
            this.logger = logger;
            this.communicationProxy = communicationProxy;
        }

        public Task Approved(string isApproved)
        {
            var player = this.GetPlayerInfo();
            this.communicationProxy.OnApprovalProvided(player.Id, isApproved);
            return Task.CompletedTask;
        }

        public Task Resolved(Dto.Suggestion suggestion)
        {
            var player = this.GetPlayerInfo();
            this.communicationProxy.OnResolutionProvided(player.Id, suggestion);
            return Task.CompletedTask;
        }

        public override async Task OnConnectedAsync()
        {
            var player = this.GetPlayerInfo();
            await base.OnConnectedAsync();
            this.communicationProxy.OnPlayerJoined(player.Id, player.Name);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var player = this.GetPlayerInfo();
            try
            {
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"Error occured on On Disconnecting for player {player.Id} {player.Name}");
            }
            
            this.communicationProxy.OnPlayerLeft(player.Id);
        }

        private Dto.PlayerInfo GetPlayerInfo()
        {
            var connectionId = this.Context.ConnectionId;
            var requestCookies = this.Context.GetHttpContext().Request.Cookies;
            if (!requestCookies.TryGetValue("userName", out var playerName))
            {
                this.logger.LogError($"For connection {connectionId} from player name was not set");
                playerName = "John Doe";
            }

            return new Dto.PlayerInfo(connectionId, playerName);
        }
    }
}