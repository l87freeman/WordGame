namespace WordGame.Game.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> logger;
        private readonly IHubContext<GameHub> hubContext;

        public GameController(ILogger<GameController> logger, IHubContext<GameHub> hubContext)
        {
            this.logger = logger;
            this.hubContext = hubContext;
        }
    }
}
