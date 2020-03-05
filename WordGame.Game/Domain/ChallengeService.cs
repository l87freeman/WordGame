namespace WordGame.Game.Domain
{
    using Interfaces;
    using Models.Challenges;
    using Models.Players;

    public class ChallengeService : IChallengeService
    {
        public Challenge CurrentChallenge { get; private set; }

        public void AddApproval(PlayerInfo player, in bool isApproved)
        {
            throw new System.NotImplementedException();
        }

        public void Suggest(PlayerInfo player, string suggestion)
        {
            throw new System.NotImplementedException();
        }

        public void NextChallenge()
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }
    }
}