namespace WordGame.Game.Dto
{
    using System.Collections.Generic;

    public class Challenge
    {
        public char Letter { get; set; }

        public List<Suggestion> Suggestions { get; set; }

        public Suggestion CurrentSuggestion { get; set; }
    }
}