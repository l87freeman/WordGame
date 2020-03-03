namespace WordGame.Game.Domain.Models
{
    using System.Collections.Generic;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Stateless;

    public class Game
    {
        private readonly ILogger<Game> logger;
        private readonly IChallengeProvider challengeProvider;
        private readonly IPlayerService playerService;

        public enum State { InProgress, Stopped }
        public enum Trigger { NoPlayers, PlayersJoined, NextPlayer }

        private State state;
        private StateMachine<State, Trigger> machine;

        public Game(ILogger<Game> logger, IChallengeProvider challengeProvider, IPlayerService playerService)
        {
            this.logger = logger;
            this.challengeProvider = challengeProvider;
            this.playerService = playerService;
            this.InitializeStateMachine();
        }

        public Challenge CurrentChallenge { get; set; }

        public List<Challenge> Challenges { get; set; } = new List<Challenge>();

        private void InitializeStateMachine()
        {
            this.state = State.Stopped;
            this.machine.Configure(State.InProgress)
                .Permit(Trigger.NoPlayers, State.Stopped)
                .PermitReentryIf(Trigger.NextPlayer, () => this.CurrentChallenge.IsSolved,
                    "Current challenge is not resolved")
                .OnEntry(this.StopGame);

            this.machine.Configure(State.Stopped)
                .Permit(Trigger.PlayersJoined, State.InProgress)
                .OnEntry(this.NextTurn);
        }

        private void NextTurn()
        {
            this.CurrentChallenge = this.challengeProvider.CreateChallenge(this.CurrentChallenge);
            this.Challenges.Add(this.CurrentChallenge);

            this.CurrentChallenge.CurrentPlayer = this.playerService.NextPlayer(this.CurrentChallenge.CurrentPlayer);
        }

        private void StopGame()
        {
            this.CurrentChallenge = null;
            this.Challenges.Clear();
        }
    }
}