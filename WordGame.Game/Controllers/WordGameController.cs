namespace WordGame.Game.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;
    using AutoMapper;
    using WordGame.Game.Domain.Interfaces;
    using WordGame.Game.Domain.Models;
    using WordGame.Game.Dtos;

    [ApiController]
    [Route("[controller]")]
    public class WordGameController : ControllerBase
    {
        private readonly ILogger<WordGameController> logger;
        private readonly IMapper mapper;
        private readonly IGameManager gameManager;

        public WordGameController(ILogger<WordGameController> logger, IMapper mapper, IGameManager gameManager)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.gameManager = gameManager;
        }

        [HttpGet("restore")]
        public async Task<GameDto> GetStoredGame()
        {
            var userKey = this.HttpContext.Connection.RemoteIpAddress.ToString();
            var game = await this.gameManager.GetGameAsync(userKey);
            return this.mapper.Map<GameDto>(game);
        }

        [HttpPost("new")]
        public async Task<GameDto> NewGame(GameDto gameDto)
        {
            var game = this.mapper.Map<Game>(gameDto);
            game = await this.gameManager.CreateGameAsync(game);

            return this.mapper.Map<GameDto>(game);
        }

        [HttpPost("resolve")]
        public async Task<GameDto> ResolveChallenge(GameDto gameDto)
        {
            var game = this.mapper.Map<Game>(gameDto);
            game = await this.gameManager.ResolveChallengeAsync(game);
            
            return this.mapper.Map<GameDto>(game);
        }
    }
}
