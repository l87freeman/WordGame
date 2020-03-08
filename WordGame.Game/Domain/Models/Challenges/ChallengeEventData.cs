namespace WordGame.Game.Domain.Models.Challenges
{
    using System.Collections.Generic;
    using Players;

    public class ChallengeEventData : GameEventArgs
    {
        public Challenge EventByChallenge { get; set; }

        public List<(Player, bool)> Approvals { get; set; }
    }
}