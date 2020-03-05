namespace WordGame.Game.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using Models.Players;
    using WordGame.Game.Controllers;
    using WordGame.Game.Domain.Models;

    public class GameManager : IGameManager
    {
        private readonly ILogger<GameManager> logger;
        private readonly IHubContext<GameHub> hubContext;
        private readonly IPlayerService playerService;
        private readonly IChallengeService challengeService;
        private readonly IMessageProvider messageProvider;


        public GameManager(ILogger<GameManager> logger, 
            IHubContext<GameHub> hubContext, 
            IPlayerService playerService,
            IChallengeService challengeService,
            IMessageProvider messageProvider)
        {
            this.logger = logger;
            this.hubContext = hubContext;
            this.playerService = playerService;
            this.challengeService = challengeService;
            this.messageProvider = messageProvider;
            this.playerService.PlayersChanged += this.NotifyPlayersChanged;
        }

        public void ApplyApproval(PlayerInfo player, bool isApproved)
        {
            this.challengeService.AddApproval(player, isApproved);
        }

        public void ApplyResolution(PlayerInfo player, string suggestion)
        {
            this.challengeService.Suggest(player, suggestion);
        }

        public void PlayerJoined(PlayerInfo player)
        {
            this.playerService.Add(player);
        }

        public void PlayerLeft(PlayerInfo player)
        {
            this.playerService.Remove(player);
        }

        private void NotifyPlayersChanged(object sender, PlayerEventData eventData)
        {
            var message = this.messageProvider.GetMessage(eventData);
            var notifyTask = this.NotifyClientsAsync(eventData.EventByPlayer.Connection, message);

            notifyTask.GetAwaiter().GetResult();
        }

        private async Task NotifyClientsAsync(string playerIdToNotNotify, string message)
        {
            await this.hubContext.Clients.AllExcept(new List<string>{ playerIdToNotNotify }).SendAsync("Notify", message);
            this.logger.LogDebug($"Message {message} was sent");
        }
    }
}