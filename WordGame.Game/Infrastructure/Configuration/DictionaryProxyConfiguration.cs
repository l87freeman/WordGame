namespace WordGame.Game.Infrastructure.Configuration
{
    using Microsoft.Extensions.Options;

    public class DictionaryProxyConfiguration : IOptions<DictionaryProxyConfiguration>
    {
        public string BaseAddress { get; set; }

        public string WordsAddress { get; set; }

        public string IsExistsAddress { get; set; }

        public DictionaryProxyConfiguration Value => this;
    }
}