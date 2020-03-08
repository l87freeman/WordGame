namespace WordGame.Game.Domain.Models.Challenges
{
    using System.Collections.Generic;
    using Players;

    public class Challenge
    {
        public Challenge(char letter, Player player)
        {
            this.Letter = letter;
            this.ChallengeFor = player;
        }

        public Player ChallengeFor { get; }

        public char Letter { get; }

        public List<Suggestion> Suggestions { get; } = new List<Suggestion>();

        public Suggestion CurrentSuggestion { get; private set; }

        public bool IsSolved =>
            this.CurrentSuggestion != null && this.CurrentSuggestion.IsValid && this.CurrentSuggestion.Approved;

        public void AddApproval(Player player, bool isApproved)
        {
            this.CurrentSuggestion.Approvals.Add((player, isApproved));
        }

        public void Suggest(Suggestion suggestion)
        {
            this.Suggestions.Add(suggestion);
            this.CurrentSuggestion = suggestion;
        }

        public void Resolve()
        {
            this.CurrentSuggestion.Approved = true;
            this.CurrentSuggestion.IsValid = true;
        }
    }
}