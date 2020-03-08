namespace WordGame.ConsoleUI.Domain.Models
{
    public class Suggestion
    {
        public string Word { get; set; }

        public bool Approved { get; set; }

        public bool IsValid { get; set; }

        public bool IsNotProvided { get; set; }

        public override string ToString()
        {
            var suggestion = $"Suggestion {this.Word} (Approved [{this.Approved}] IsValid [{this.IsValid}])";
            return suggestion;
        }
    }
}