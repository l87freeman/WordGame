namespace WordGame.Game.Dto
{
    public class Suggestion
    {
        public string Word { get; set; }

        public bool Approved { get; set; }

        public bool IsValid { get; set; }

        public bool IsNotProvided { get; set; }
    }
}