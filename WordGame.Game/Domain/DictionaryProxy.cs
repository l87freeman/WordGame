namespace WordGame.Game.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Dto;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    public class DictionaryProxy : IDictionaryProvider
    {
        private readonly ILogger<DictionaryProxy> logger;
        private readonly DictionaryProxyConfiguration config;

        public DictionaryProxy(IOptions<DictionaryProxyConfiguration> options, ILogger<DictionaryProxy> logger)
        {
            this.logger = logger;
            this.config = options.Value;
        }
        public List<string> GetWords(char challengeLetter)
        {
            var uriSting = string.Format($"{this.config.BaseAddress}{this.config.WordsAddress}", challengeLetter);
            this.logger.LogDebug($"Call for {uriSting}");
            var words = this.GetDataAsync<List<string>>(new Uri(uriSting)).GetAwaiter().GetResult();
            return words;
        }

        public bool IsWordExists(string word)
        {
            var uriSting = string.Format($"{this.config.BaseAddress}{this.config.IsExistsAddress}", word);
            this.logger.LogDebug($"Call for {uriSting}");
            var isExists = this.GetDataAsync<bool>(new Uri(uriSting)).GetAwaiter().GetResult();
            return isExists;
        }

        private async Task<T> GetDataAsync<T>(Uri uri)
        {
            var stringData = await this.ReadFromRemoteAsync(uri);
            var data = this.Convert<T>(stringData);

            return data;
        }

        private async Task<string> ReadFromRemoteAsync(Uri uri)
        {
            string resultString = string.Empty;
            using (var httpClient = new HttpClient(new HttpClientHandler()))
            {
                try
                {
                    resultString = await httpClient.GetStringAsync(uri);
                }
                catch (Exception e)
                {
                    this.logger.LogError(e, "Was not able to read from remote");
                }

            }
            return resultString;
        }

        private T Convert<T>(string resultString)
        {
            T result = default;
            try
            {
                result = JsonConvert.DeserializeObject<T>(resultString);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"Was not able to DeserializeObject from string {resultString}");
            }

            return result;
        }
    }
}