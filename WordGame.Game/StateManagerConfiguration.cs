namespace WordGame.Game
{
    using Microsoft.Extensions.Options;

    public class StateManagerConfiguration : IOptions<StateManagerConfiguration>
    {
        public string Address { get; set; }

        public StateManagerConfiguration Value => this;
    }
}