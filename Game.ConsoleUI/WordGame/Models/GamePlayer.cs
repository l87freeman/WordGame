namespace Game.ConsoleUI.WordGame.Models
{
    using System;
    using Newtonsoft.Json;

    public class GamePlayer
    {
        [JsonConstructor]
        protected GamePlayer(string name, string id)
        {
            this.Name = name;
            this.Id = id;
        }

        public GamePlayer(string name)
        {
            this.Name = name;
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; }

        public string Name { get; }

        public override string ToString()
        {
            return $"Player: {this.Name} (Id: {this.Id})";
        }
    }
}