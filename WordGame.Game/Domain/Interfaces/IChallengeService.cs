namespace WordGame.Game.Domain.Interfaces
{
    using System.Collections.Generic;
    using Models.Challenges;
    using Models.Players;

    public interface IChallengeService
    {
        Challenge CurrentChallenge { get; } 
        
        List<Challenge> Challenges { get; }

        void SuggestWithValidation(Suggestion suggestion);
        
        void NextChallengeFor(Player player);

        void AddApproval(Player player, bool isApproved);

        void Reset();
    }
}