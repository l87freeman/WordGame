namespace WordGame.Game.Domain.Models.Players
{
    public class PlayerEventData : GameEventArgs
    {
        public int PlayersCount { get; set; }
    }
}