namespace WordGame.Game.Domain.Models
{
    using System.Collections.Generic;

    public class Challenge
    {
        public char Letter { get; set; }

        public List<Suggestion> Suggestions { get; set; }

        public Suggestion CurrentSuggestion { get; set; }

        public bool IsSolved =>
            this.CurrentSuggestion != null && this.CurrentSuggestion.IsValid && this.CurrentSuggestion.Approved;

        public PlayerInfo CurrentPlayer { get; set; }
    }
}