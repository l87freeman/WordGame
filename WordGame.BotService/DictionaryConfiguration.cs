namespace WordGame.BotService
{
    using Microsoft.Extensions.Options;

    public class DictionaryConfiguration : IOptions<DictionaryConfiguration>
    {
        public string Address { get; set; }

        public DictionaryConfiguration Value => this;
    }
}