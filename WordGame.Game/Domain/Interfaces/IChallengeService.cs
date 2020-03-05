namespace WordGame.Game.Domain.Interfaces
{
    using Models.Challenges;
    using Models.Players;

    public interface IChallengeService
    {
        Challenge CurrentChallenge { get; }

        void AddApproval(PlayerInfo player, in bool isApproved);

        void Suggest(PlayerInfo player, string suggestion);
        
        void NextChallenge();

        void Reset();
    }
}