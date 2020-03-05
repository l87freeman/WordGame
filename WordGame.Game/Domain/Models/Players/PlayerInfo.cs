namespace WordGame.Game.Domain.Models.Players
{
    public class PlayerInfo
    {
        public PlayerInfo(string connection, string name)
        {
            Connection = connection;
            Name = name;
        }

        public string Connection { get; }

        public string Name { get; }

        public override string ToString()
        {
            var info = $"Player {this.Name} ({this.Connection})";
            return info;
        }
    }
}