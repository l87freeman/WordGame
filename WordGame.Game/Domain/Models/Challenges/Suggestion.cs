namespace WordGame.Game.Domain.Models.Challenges
{
    using System.Collections.Generic;
    using Players;

    public class Suggestion
    {
        public List<(Player player, bool isApproved)> Approvals { get; } = new List<(Player, bool)>();

        public Suggestion(string word)
        {
            this.Word = word;
        }

        public string Word { get; }

        public bool Approved { get; set; }

        public bool IsValid { get; set; }

        public bool IsNotProvided { get; set; }
    }
}