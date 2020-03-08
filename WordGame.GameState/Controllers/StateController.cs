namespace WordGame.GameState.Controllers
{
    using System.Threading.Tasks;
    using Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Storage.Interfaces;

    [ApiController]
    [Route("[controller]")]
    public class StateController : ControllerBase
    {

        private readonly ILogger<StateController> logger;
        private readonly IStateStorage stateStorage;

        public StateController(ILogger<StateController> logger, IStateStorage stateStorage)
        {
            this.logger = logger;
            this.stateStorage = stateStorage;
        }

        [HttpPost]
        public async Task Save([FromBody] GameDto state)
        {
            this.logger.LogDebug($"Saving state for game {state.CurrentPlayer?.Id ?? "No player"}");
            await this.stateStorage.SaveAsync(state);
        }
    }
}
