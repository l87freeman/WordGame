namespace WordGame.Game.Domain.Interfaces
{
    using Models;
    using Models.Challenges;
    using Models.Players;

    public interface IMessageProvider
    {
        string GetMessage(PlayerEventData eventData);

        string GetMessage(ChallengeEventData eventData);
    }
}