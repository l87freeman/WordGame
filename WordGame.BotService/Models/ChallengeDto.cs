namespace WordGame.BotService.Models
{
    using System.Collections.Generic;

    public class ChallengeDto
    {
        public char Challenge { get; set; }

        public List<Suggestion> Suggestions { get; set; }
    }
}