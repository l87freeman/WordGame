namespace Game.ConsoleUI.Game.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    public class GameState
    {
        private GameChallenge gameChallenge;
        private GamePlayer currentPlayer;

        public event EventHandler<EventArgs> StateChanged;

        public List<GamePlayer> Players { get; } = new List<GamePlayer>();

        public GamePlayer CurrentPlayer
        {
            get => currentPlayer;
            set
            {
                this.currentPlayer = value;
                this.StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public GameChallenge CurrentChallenge
        {
            get => gameChallenge;
            set
            {
                this.gameChallenge = value;
                this.StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public List<GameChallenge> Challenges { get; } = new List<GameChallenge>();

        public override string ToString()
        {
            var separator = Environment.NewLine;
            var allPlayers = string.Join(separator, this.Players.Select(player => player.ToString()));
            var gameStateString =
                $"Current challenge: {this.CurrentChallenge}{separator}Current player: {this.CurrentPlayer}{separator}All players: {allPlayers}";
            return gameStateString;
        }
    }
}