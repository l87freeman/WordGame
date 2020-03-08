namespace WordGame.BotService
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    public class DictionaryProxy : IDictionaryProxy
    {
        private readonly ILogger<DictionaryProxy> logger;
        private readonly string address;

        public DictionaryProxy(ILogger<DictionaryProxy> logger, IOptions<DictionaryConfiguration> config)
        {
            this.logger = logger;
            this.address = config.Value.Address;
        }

        public async Task<ISet<string>> GetWords(char letter)
        {
            var words = await this.GetWordsFromDictionaryAsync(letter);
            return words;
        }

        private async Task<ISet<string>> GetWordsFromDictionaryAsync(char letter)
        {
            var serString = await this.ReadFromRemoteAsync(letter);
            var result = this.Convert(serString);

            return result;
        }

        private async Task<string> ReadFromRemoteAsync(char letter)
        {
            string resultString = string.Empty;
            using (var httpClient = new HttpClient(new HttpClientHandler()))
            {
                try
                {
                    resultString = await httpClient.GetStringAsync(string.Format(this.address, letter));
                    
                }
                catch (Exception e)
                {
                    this.logger.LogError(e, "Was not able to read from remote");
                }
                
            }
            return resultString;
        }

        private ISet<string> Convert(string resultString)
        {
            HashSet<string> result = new HashSet<string>();
            try
            {
                result = JsonConvert.DeserializeObject<HashSet<string>>(resultString);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"Was not able to DeserializeObject from string {resultString}");
            }

            return result;
        }
    }
}