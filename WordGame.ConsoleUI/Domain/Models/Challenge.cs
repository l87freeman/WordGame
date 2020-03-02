namespace WordGame.ConsoleUI.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Serialization;

    public class Challenge
    {
        public char Letter { get; set; }

        public List<Suggestion> Suggestions { get; set; }

        public Suggestion CurrentSuggestion { get; set; }

        [JsonIgnore]
        public bool IsResolved => this.CurrentSuggestion != null && this.CurrentSuggestion.IsValid && this.CurrentSuggestion.Approved;

        public override string ToString()
        {
            var suggestion = this.CurrentSuggestion == null ? "----" : this.Suggestions.ToString();
            var historyString = string.Join($@"{Environment.NewLine}\t\t\t", this.Suggestions.Select(s => s.ToString()));
            var challenge = $"Challenge for {this.Letter} letter is {this.IsResolved}, was suggested {suggestion}.{Environment.NewLine}{historyString}";
            return challenge;
        }
    }
}