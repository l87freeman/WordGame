namespace Game.ConsoleUI.WordGame.Models
{
    using Newtonsoft.Json;

    public class GamePlayerBot : GamePlayer
    {
        [JsonConstructor]
        private GamePlayerBot(string name, string id) : base(name, id) { }

        public GamePlayerBot() : base("Roboto") { }
    }
}