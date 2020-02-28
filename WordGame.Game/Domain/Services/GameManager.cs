namespace WordGame.Game.Domain.Services
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using WordGame.Game.Domain.Interfaces;
    using WordGame.Game.Domain.Models;

    public class GameManager : IGameManager
    {
        private readonly ILogger<GameManager> logger;
        private readonly IGameStateProvider gameStateProvider;
        private readonly IGameChallengeValidator validator;

        public GameManager(ILogger<GameManager> logger, IGameStateProvider gameStateProvider, IGameChallengeValidator validator)
        {
            this.logger = logger;
            this.gameStateProvider = gameStateProvider;
            this.validator = validator;
        }

        public Task<Game> GetGameAsync(string userIp)
        {
            return this.gameStateProvider.TryRestoreGame(userIp);
        }

        public Task<Game> CreateGameAsync(Game game)
        {
           return this.gameStateProvider.GetOrCreate(game);
        }

        public async Task<Game> ResolveChallengeAsync(Game game)
        {
            game = await this.gameStateProvider.SynchronizeAsync(game);
            if (this.validator.ValidateResolution(game))
            {
                await this.gameStateProvider.SynchronizeAsync(game);
            }

            return game;
        }
    }
}