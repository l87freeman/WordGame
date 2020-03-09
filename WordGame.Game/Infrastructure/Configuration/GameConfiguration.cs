namespace WordGame.Game.Infrastructure.Configuration
{
    using Microsoft.Extensions.Options;

    public class GameConfiguration : IOptions<GameConfiguration>
    {
        public string StateServiceAddress { get; set; }

        public string StateServiceRestoreUri { get; set; }

        public GameConfiguration Value => this;
    }
}