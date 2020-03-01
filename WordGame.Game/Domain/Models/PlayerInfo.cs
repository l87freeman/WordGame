namespace WordGame.Game.Domain.Models
{
    public class PlayerInfo
    {
        public string Connection { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            var info = $"Player {this.Name} ({this.Connection})";
            return info;
        }
    }
}