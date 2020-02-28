namespace WordGame.Game.Domain.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Challenge
    {
        public Challenge(string id, char letter)
        {
            this.Id = id;
            this.Letter = letter;
        }

        public string Id { get; }

        public char Letter { get; }

        public List<Suggestion> Suggestions { get; } = new List<Suggestion>();

        public bool IsActive { get; set; }

        public bool Resolved => this.Suggestions.Any(sug => sug.Approved && sug.Valid);
    }
}