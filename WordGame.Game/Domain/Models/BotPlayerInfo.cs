namespace WordGame.Game.Domain.Models
{
    using Players;

    public class BotPlayerInfo : PlayerInfo
    {
        public BotPlayerInfo() : base("This is a bot", "-=Roboto=-")
        {
        }
    }
}