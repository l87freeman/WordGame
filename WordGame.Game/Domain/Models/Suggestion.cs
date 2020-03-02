namespace WordGame.Game.Domain.Models
{
    public class Suggestion
    {
        public string Word { get; set; }

        public bool Approved { get; set; }

        public bool IsValid { get; set; }
    }
}