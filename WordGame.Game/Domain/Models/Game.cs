namespace WordGame.Game.Domain.Models
{
    using System.Collections.Generic;
    using Challenges;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Stateless;

    public class Game
    {
        private readonly ILogger<Game> logger;
        private readonly IChallengeService challengeService;
        private readonly IPlayerService playerService;

        public enum State { InProgress, Stopped }
        public enum Trigger { NoPlayers, OnePlayerLeft, PlayersJoined, NextPlayer }

        private State state;
        private StateMachine<State, Trigger> machine;

        public Game(ILogger<Game> logger,
            IChallengeService challengeService,
            IPlayerService playerService)
        {
            this.logger = logger;
            this.challengeService = challengeService;
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
                .Permit(Trigger.OnePlayerLeft, State.Stopped)
                .PermitReentryIf(Trigger.NextPlayer, () => this.CurrentChallenge.IsSolved,
                    "Current challenge is not resolved")
                .OnEntry(this.OnNextTurn);

            this.machine.Configure(State.Stopped)
                .Permit(Trigger.PlayersJoined, State.InProgress)
                .OnEntry(this.OnStopGame);
        }

        private void OnNextTurn()
        {
            this.CurrentChallenge = this.challengeService.CreateChallenge(this.CurrentChallenge);
            this.Challenges.Add(this.CurrentChallenge);
            this.playerService.NextPlayer();
        }

        private void OnStopGame()
        {
            this.CurrentChallenge = null;
            this.Challenges.Clear();
        }
    }
}