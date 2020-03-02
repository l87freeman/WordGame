namespace WordGame.ConsoleUI.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Game
    {
        public List<Challenge> Challenges { get; set; }

        public string CurrentPlayer { get; set; }

        public Challenge CurrentChallenge { get; set; }

        public override string ToString()
        {
            //TODO create a formatter for this and utilize string builder and recursion
            var challengesSting = string.Join($@"{Environment.NewLine}\t", this.Challenges.Select(c => c.ToString()));
            var game = $"Game on {this.CurrentChallenge} for current player {this.CurrentPlayer}.{Environment.NewLine}Challenges history:{Environment.NewLine}{challengesSting}";
             
            return game;
        }
    }
}