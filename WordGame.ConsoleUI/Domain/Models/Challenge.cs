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

        public override string ToString()
        {
            var suggestion = this.CurrentSuggestion == null ? "----" : this.CurrentSuggestion.ToString();
            var historyString = string.Join($@"{Environment.NewLine} \t\t\t", this.Suggestions.Select(s => s.ToString()));
            var challenge = $"{Environment.NewLine}Challenge for letter [{this.Letter}] letter, Approved: [{this.CurrentSuggestion?.Approved}], IsValid: [{this.CurrentSuggestion?.IsValid}], IsNotProvided: [{this.CurrentSuggestion?.IsNotProvided}]" 
                            + $" was suggested [{suggestion}].{Environment.NewLine}" 
                            + $"Suggestion history:{Environment.NewLine}{historyString}";
            return challenge;
        }
    }
}