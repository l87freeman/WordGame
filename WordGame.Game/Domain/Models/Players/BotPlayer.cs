namespace WordGame.Game.Domain.Models.Players
{
    public class BotPlayer : Player
    {
        public const string BotPlayerId = "-=Roboto=-";
        public BotPlayer() : base(BotPlayerId, "#Robot Wall-E#")
        {
        }
    }
}