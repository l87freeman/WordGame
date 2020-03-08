namespace WordGame.Game.Domain
{
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Models;

    public class GameServiceFactory : IGameServiceFactory
    {
        private readonly ILogger<GameService> logger;
        private readonly IPlayerService playerService;
        private readonly IChallengeService challengeService;
        private IGameService gameService;

        public GameServiceFactory(ILogger<GameService> logger, IPlayerService playerService, IChallengeService challengeService)
        {
            this.logger = logger;
            this.playerService = playerService;
            this.challengeService = challengeService;
        }

        public IGameService Create(ICommunicationProxy communicationProxy)
        {
            lock (this)
            {
                this.gameService ??= new GameService(this.logger, communicationProxy, this.playerService, this.challengeService);
            }
            return this.gameService;
        }
    }
}