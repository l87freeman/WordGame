namespace WordGame.ConsoleUI
{
    using Microsoft.Extensions.Options;

    public class GameConfiguration : IOptions<GameConfiguration>
    {
        public string ServerAddress { get; set; }

        public GameConfiguration Value => this;
    }
}