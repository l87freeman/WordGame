namespace WordGame.Game.Domain.Models
{
    using System.Collections.Generic;

    public class Game
    {
        public Game(string id)
        {
            this.Id = id;
        }

        public string Id { get; }

        public List<Player> Players { get; } = new List<Player>();

        public List<Challenge> Challenges { get; } = new List<Challenge>();
    }
}