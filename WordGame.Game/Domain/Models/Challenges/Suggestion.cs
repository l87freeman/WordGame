namespace WordGame.Game.Domain.Models.Challenges
{
    public class Suggestion
    {
        public Suggestion(string word)
        {
            Word = word;
        }

        public string Word { get; }

        public bool Approved { get; set; }

        public bool IsValid { get; set; }
    }
}