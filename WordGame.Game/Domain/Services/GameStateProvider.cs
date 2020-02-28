namespace WordGame.Game.Domain.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using WordGame.Game.Domain.Interfaces;
    using WordGame.Game.Domain.Models;
    using WordGame.Game.Dtos;
    using WordGame.Game.Infrastructure.Interfaces;

    public class GameStateProvider : IGameStateProvider
    {
        private readonly ILogger<GameStateProvider> logger;
        private readonly IMapper mapper;
        private readonly IRemoteService remoteService;
        private readonly GameConfiguration config;

        public GameStateProvider(ILogger<GameStateProvider> logger, 
            IOptions<GameConfiguration> config, 
            IMapper mapper, 
            IRemoteService remoteService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.remoteService = remoteService;
            this.config = config.Value;

            this.remoteService.SetBaseUri(this.config.StateServiceAddress);
        }

        public async Task<Game> TryRestoreGame(string gameId)
        {
            var game = await this.CallRemoteAsync(this.config.StateServiceRestoreUri + gameId,
                () => this.remoteService.GetAsync<GameDto>(this.config.StateServiceRestoreUri));
            
            return game;
        }

        public Task<Game> GetOrCreate(Game game)
        {
            throw new System.NotImplementedException();
        }

        public Task<Game> SynchronizeAsync(Game game)
        {
            throw new System.NotImplementedException();
        }

        private async Task<Game> CallRemoteAsync(string request, Func<Task<GameDto>> serviceFunc)
        {
            Game game = null;
            if (!string.IsNullOrWhiteSpace(request))
            {
                var gameDto = await serviceFunc();
                game = this.mapper.Map<Game>(gameDto);
            }

            return game;
        }
    }
}