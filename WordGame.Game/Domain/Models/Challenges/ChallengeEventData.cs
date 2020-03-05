namespace WordGame.Game.Domain.Models.Challenges
{
    using System.Collections.Generic;

    public class ChallengeEventData
    {
        public Challenge EventByChallenge { get; set; }

        public ChallengeEventType EventType { get; set; }

        public Dictionary<string, bool> Approvals { get; set; }
    }
}