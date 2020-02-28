namespace WordGame.Game.Domain.Models
{
    public class Player
    {
        public Player(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public string Id { get; }

        public string Name { get; }

        public bool IsActive { get; set; }
    }
}