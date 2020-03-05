namespace WordGame.Game.Domain.Interfaces
{
    using Models;
    using Models.Challenges;

    public interface IChallengeService
    {
        Challenge CreateChallenge(Challenge currentChallenge);
    }
}