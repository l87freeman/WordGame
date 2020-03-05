namespace WordGame.Game.Domain.Models.Challenges
{
    using System.Collections.Generic;

    public class Challenge
    {
        public Challenge(char letter)
        {
            Letter = letter;
        }

        public char Letter { get; }

        public List<Suggestion> Suggestions { get; } = new List<Suggestion>();

        public Suggestion CurrentSuggestion { get; private set; }

        public bool IsSolved =>
            this.CurrentSuggestion != null && this.CurrentSuggestion.IsValid && this.CurrentSuggestion.Approved;

        public void Suggest(string word)
        {
            var suggestion = new Suggestion(word);
            this.Suggestions.Add(suggestion);
            this.CurrentSuggestion = suggestion;
        }

        public override string ToString()
        {
            var suggestion = this.CurrentSuggestion == null ? string.Empty : $", suggestion: {this.CurrentSuggestion}";
            var challenge =
                $"Challenge for letter {this.Letter} is resolved: {this.IsSolved}{suggestion}";
            return challenge;
        }
    }
}