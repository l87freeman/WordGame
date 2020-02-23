namespace Game.ConsoleUI.Infrastructure
{
    using Microsoft.Extensions.Options;

    public class GameConfiguration : IOptions<GameConfiguration>
    {
        public string DictionaryFolder { get; set; }

        public string DictionaryFile { get; set; }

        public string StorageFolder { get; set; }

        public GameConfiguration Value => this;
    }
}