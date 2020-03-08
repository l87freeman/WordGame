namespace WordGame.Game.Dto
{
    using System.Collections.Generic;

    public class GameDto
    {
        public List<Challenge> Challenges { get; set; }

        public PlayerInfo CurrentPlayer { get; set; }

        public Challenge CurrentChallenge { get; set; }
    }
}