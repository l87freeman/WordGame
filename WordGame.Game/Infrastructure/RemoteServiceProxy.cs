namespace WordGame.Game.Infrastructure
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using WordGame.Game.Infrastructure.Interfaces;

    public class RemoteServiceProxy : IRemoteService
    {
        private readonly ILogger<RemoteServiceProxy> logger;
        private readonly HttpClient httpClient;

        public RemoteServiceProxy(ILogger<RemoteServiceProxy> logger)
        {
            this.logger = logger;
            this.httpClient = new HttpClient(new HttpClientHandler());
        }

        public void SetBaseUri(string baseUri)
        {
            this.httpClient.BaseAddress = new Uri(baseUri);
        }

        public async Task<TResponse> GetAsync<TResponse>(string path) where TResponse : class
        {
            this.logger.LogDebug($"Publishing get request to {path}");
            var response = await this.httpClient.GetAsync(path);
            var dto = await this.GetResponseDto<TResponse>(response);

            this.logger.LogDebug($"Received response {JsonConvert.SerializeObject(dto)}");
            return dto;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string path, TRequest data) where TRequest : class where TResponse : class
        {
            this.logger.LogDebug($"Publishing post request to {path}");
            var response = await this.httpClient.PostAsync(path, new StringContent(JsonConvert.SerializeObject(data)));
            var dto = await this.GetResponseDto<TResponse>(response);

            this.logger.LogDebug($"Received response {JsonConvert.SerializeObject(dto)}");
            return dto;
        }

        public void Dispose()
        {
            this.httpClient.Dispose();
        }

        private async Task<TResponse> GetResponseDto<TResponse>(HttpResponseMessage responseMessage) where TResponse : class
        {
            TResponse dto = default;
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseString = await responseMessage.Content.ReadAsStringAsync();
                dto = JsonConvert.DeserializeObject<TResponse>(responseString);
            }

            return dto;
        }
    }
}