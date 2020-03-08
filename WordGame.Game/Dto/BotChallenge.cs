namespace WordGame.Game.Dto
{
    using System.Collections.Generic;

    public class BotChallenge
    {
        public char Challenge { get; set; }

        public List<Dto.Suggestion> Suggestions { get; set; }
    }
}