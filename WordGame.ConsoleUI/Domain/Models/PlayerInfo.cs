namespace WordGame.ConsoleUI.Domain.Models
{
    public class PlayerInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public override string ToString()
        {
            var player = $"Player {this.Name} ({this.Id} ) is active [{this.IsActive}]";
            return player;
        }
    }
}