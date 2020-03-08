namespace WordGame.Game.Domain.Interfaces
{
    using System.Collections.Generic;
    using Models.Challenges;

    public interface IChallengeResolutionValidator
    {
        void CheckResolution(Suggestion resolutionCandidate, List<Suggestion> history);
    }
}