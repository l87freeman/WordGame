namespace Game.ConsoleUI.Game.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    public class GameState
    {
        [JsonProperty]
        private int currentPlayerIndex = -1;
        [JsonProperty]
        private List<GameChallenge> challenges = new List<GameChallenge>();
        [JsonProperty]
        private List<GamePlayer> players = new List<GamePlayer>();

        public event EventHandler<EventArgs> StateChanged;

        [JsonIgnore]
        public GamePlayer CurrentPlayer => this.currentPlayerIndex < 0 ? null : this.players[this.currentPlayerIndex];

        [JsonIgnore]
        public GameChallenge CurrentChallenge => this.challenges.LastOrDefault();

        [JsonIgnore]
        public List<GamePlayer> Players => this.players.ToList();

        [JsonIgnore]
        public List<GameChallenge> ChallengeHistory => this.challenges.ToList();

        public GameChallenge NextChallenge()
        {
            var challenge = this.CreateChallenge();
            this.ShiftToNextPlayer();
            this.StateChanged?.Invoke(this, EventArgs.Empty);

            return challenge;
        }

        public void AddPlayer(GamePlayer player)
        {
            this.players.Add(player);
        }

        [JsonIgnore]
        public bool IsChallengeResolved => this.CurrentChallenge == null || this.CurrentChallenge.ChallengeResolution != null;

        private GameChallenge CreateChallenge()
        {
            var letter = this.CurrentChallenge?.ChallengeResolution?.Last() ?? 'a';
            var challenge = new GameChallenge(letter);
            this.challenges.Add(challenge);

            return challenge;
        }

        private void ShiftToNextPlayer()
        {
            var lastPlayerIndex = this.players.Count - 1;
            if (++this.currentPlayerIndex > lastPlayerIndex)
            {
                this.currentPlayerIndex = 0;
            }
        }

        public override string ToString()
        {
            var separator = Environment.NewLine;
            var allPlayers = string.Join(separator, this.players.Select(player => player.ToString()));
            var gameStateString =
                $"Current challenge: {this.CurrentChallenge}{separator}Current player: {this.CurrentPlayer}{separator}All players: {allPlayers}";
            return gameStateString;
        }

        public void ResolveChallenge()
        {
            this.CurrentChallenge.Resolve();
        }
    }
}