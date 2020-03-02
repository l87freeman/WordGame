namespace WordGame.Game.Domain
{
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
        private readonly IPlayerService playerService;


        public GameManager(ILogger<GameManager> logger, IHubContext<GameHub> hubContext, IPlayerService playerService)
        {
            this.logger = logger;
            this.hubContext = hubContext;
            this.playerService = playerService;
            this.playerService.PlayersChanged += this.NotifyPlayersChanged;
        }

        public Task ApplyApprovalAsync(PlayerInfo player, bool isApproved)
        {
            return Task.CompletedTask;
        }

        public Task ApplyResolutionAsync(PlayerInfo player, string message)
        {
            return Task.CompletedTask;
        }


        public void PlayerJoined(PlayerInfo player)
        {
            this.playerService.Add(player);
        }

        public void PlayerLeft(PlayerInfo player)
        {
            this.playerService.Remove(player);
        }

        private void NotifyPlayersChanged(object sender, string message)
        {
            this.hubContext.Clients.All.SendAsync("Notify", message).GetAwaiter().GetResult();
        }
    }
}