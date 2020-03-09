namespace WordGame.Game.Infrastructure.Services
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Configuration;
    using Dto;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    public class StateManager : IStateManager
    {
        private readonly ILogger<StateManager> logger;
        private readonly string address;

        public StateManager(IOptions<StateManagerConfiguration> config, ILogger<StateManager> logger)
        {
            this.logger = logger;
            this.address = config.Value.Address;
        }

        public void SaveState(GameDto gameDto)
        {
            this.logger.LogDebug($"Storing state for game [{gameDto.CurrentPlayer?.Name ?? "No player"}] as a current player");
            this.PostToRemoteAsync(gameDto).GetAwaiter().GetResult();
        }

        private async Task PostToRemoteAsync(GameDto dto)
        {
            using (var httpClient = new HttpClient(new HttpClientHandler()))
            {
                try
                {
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, this.address)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8,
                            "application/json")
                    };
                    var response = await httpClient.SendAsync(requestMessage);
                    if (!response.IsSuccessStatusCode)
                    {
                        this.logger.LogError($"Error code [{response.StatusCode}] on attempt to reach remote address [{this.address}]");
                    }
                    else
                    {
                        this.logger.LogDebug($"State was stored");
                    }

                }
                catch (Exception e)
                {
                    this.logger.LogError(e, "Was not able to read from remote");
                }
            }
        }
    }
}