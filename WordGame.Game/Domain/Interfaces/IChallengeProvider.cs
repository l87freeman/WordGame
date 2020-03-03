namespace WordGame.Game.Domain.Interfaces
{
    using Models;

    public interface IChallengeProvider
    {
        Challenge CreateChallenge(Challenge currentChallenge);
    }
}