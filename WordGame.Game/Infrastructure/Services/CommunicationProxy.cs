namespace WordGame.Game.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Controllers;
    using Domain.Interfaces;
    using Domain.Models.Challenges;
    using Domain.Models.Players;
    using Interfaces;
    using Microsoft.AspNetCore.SignalR;

    public class CommunicationProxy : ICommunicationProxy
    {
        private readonly IHubContext<GameHub> hubContext;
        private readonly IGameService gameService;
        private readonly IMapper mapper;
        private readonly IBotService botService;
        private readonly IStateManager stateManager;

        public CommunicationProxy(IHubContext<GameHub> hubContext, 
            IGameServiceFactory factory, 
            IMapper mapper, 
            IBotService botService,
            IStateManager stateManager)
        {
            this.hubContext = hubContext;
            this.gameService = factory.Create(this);
            this.mapper = mapper;
            this.botService = botService;
            this.stateManager = stateManager;

            this.botService.OnResolutionProvided(this.OnResolutionProvided);
            this.botService.OnApprovalProvided(this.OnApprovalProvided);
        }

        public void Notify(List<Player> players, string message)
        {
            this.SendToPlayer(players, "Notify", message);
        }

        public void NewChallenge(Player player, Challenge challenge)
        {
            var challengeDto = this.mapper.Map<Dto.Challenge>(challenge);
            this.SendToPlayer(new List<Player>{ player }, "NewChallenge", challengeDto);
            this.botService.NewChallenge();
        }

        public void NeedApproval(List<Player> players, Suggestion suggestion)
        {
            var suggestionDto = this.mapper.Map<Dto.Suggestion>(suggestion);
            this.SendToPlayer(players, "NeedApproval", suggestionDto);
            this.botService.NeedApproval(suggestionDto);
        }

        public void GameUpdated(Challenge currentChallenge, Player player, List<Challenge> challenges)
        {
            var gameDto = new Dto.GameDto
            {
                CurrentChallenge = this.mapper.Map<Dto.Challenge>(currentChallenge),
                CurrentPlayer = this.mapper.Map<Dto.PlayerInfo>(player),
                Challenges = this.mapper.Map<List<Dto.Challenge>>(challenges)
            };

            this.hubContext.Clients.All.SendAsync("GameUpdated", gameDto);
            this.stateManager.SaveState(gameDto);
        }

        public void OnApprovalProvided(string playerId, string isApproved)
        {
            if(!bool.TryParse(isApproved, out var isApprovedBool))
            {
                throw new InvalidOperationException($"Not boolean value [{isApproved}] was provided by player [{playerId}]");
            }
            this.gameService.OnApprovalProvided(playerId, isApprovedBool);
        }

        public void OnResolutionProvided(string playerId, Dto.Suggestion suggestion)
        {
            var domainSuggestion = this.mapper.Map<Suggestion>(suggestion);
            this.gameService.OnResolutionProvided(playerId, domainSuggestion);
        }

        public void OnPlayerJoined(string playerId, string playerName)
        {
            this.gameService.OnPlayerJoined(playerId, playerName);
        }

        public void OnPlayerLeft(string playerId)
        {
            this.gameService.OnPlayerLeft(playerId);
        }

        private void SendToPlayer<T>(List<Player> players, string eventName, T data)
        {
            Parallel.ForEach(players, p =>
            {
                var client = this.hubContext.Clients.Client(p.Id);
                client.SendAsync(eventName, data);
            });
        }
    }
}