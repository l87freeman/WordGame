namespace WordGame.Game.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Interfaces;
    using Domain.Models;
    using Domain.Models.Challenges;
    using Domain.Models.Players;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Stateless;

    public class GameService : IGameService
    {
        private readonly ILogger<GameService> logger;
        private readonly ICommunicationProxy communicationProxy;
        private readonly IPlayerService playerService;
        private readonly IChallengeService challengeService;

        private StateMachine<GameState, EventType> gameStateMachine;
        private StateMachine<ChallengeState, EventType> challengeStateMachine;

        private GameState gameState;
        private ChallengeState challengeState;

        public enum GameState
        {
            Stopped, InProgress
        }

        public enum ChallengeState
        {
            NewChallenge, WaitingForResolution, ChallengeUnresolved, ResolutionNotValid, WaitingForApprovals, ApproveDeclined, ChallengeResolved
        }

        public GameService(
            ILogger<GameService> logger,
            ICommunicationProxy communicationProxy,
            IPlayerService playerService,
            IChallengeService challengeService)
        {
            this.logger = logger;
            this.communicationProxy = communicationProxy;
            this.playerService = playerService;
            this.challengeService = challengeService;

            this.InitializeGameStateMachine();
        }

        private void InitializeGameStateMachine()
        {
            this.gameState = GameState.Stopped;
            this.gameStateMachine = new StateMachine<GameState, EventType>(() => this.gameState, s => this.gameState = s, FiringMode.Immediate);

            this.gameStateMachine.Configure(GameState.Stopped)
                .Permit(EventType.PlayerJoined, GameState.InProgress)
                .OnEntry(this.OnGameStopped);

            this.gameStateMachine.Configure(GameState.InProgress)
                .PermitIf(EventType.PlayerLeft, GameState.Stopped, () => this.playerService.ActivePlayers.Count <= 1)
                .OnEntry(this.OnGameStarted);
        }

        private void InitializeChallengeStateMachine()
        {
            this.challengeState = ChallengeState.NewChallenge;
            this.challengeStateMachine = new StateMachine<ChallengeState, EventType>(() => this.challengeState, s => this.challengeState = s, FiringMode.Queued);

            this.challengeStateMachine.Configure(ChallengeState.NewChallenge)
                .Permit(EventType.NewChallenge, ChallengeState.WaitingForResolution)
                .OnEntry(this.OnNewChallenge);

            this.challengeStateMachine.Configure(ChallengeState.WaitingForResolution)
                .PermitIf(EventType.ResolutionProvided, ChallengeState.ResolutionNotValid,
                    () => !this.challengeService.CurrentChallenge.CurrentSuggestion.IsValid)
                .PermitIf(EventType.PlayerLeft, ChallengeState.ChallengeUnresolved, () => !this.playerService.CurrentPlayer.IsActive)
                .PermitIf(EventType.ResolutionProvided, ChallengeState.ChallengeUnresolved, () => this.challengeService.CurrentChallenge.CurrentSuggestion.IsNotProvided)
                .Permit(EventType.ResolutionProvided, ChallengeState.WaitingForApprovals)
                .OnEntry(this.OnWaitingForResolution);

            this.challengeStateMachine.Configure(ChallengeState.ResolutionNotValid)
                .Permit(EventType.ResolutionChecked, ChallengeState.WaitingForResolution)
                .PermitIf(EventType.PlayerLeft, ChallengeState.ChallengeUnresolved, () => !this.playerService.CurrentPlayer.IsActive)
                .OnEntry(this.OnResolutionNotValid);

            this.challengeStateMachine.Configure(ChallengeState.WaitingForApprovals)
                .PermitIf(EventType.ApproveReceived, ChallengeState.ChallengeResolved,
                    () =>
                    {
                        var allApproved = this.playerService.ActivePlayersNoCurrent
                            .All(p => this.challengeService.CurrentChallenge.CurrentSuggestion.Approvals.Any(ap =>
                                ap.isApproved && ap.player.Equals(p)));
                        return allApproved;
                    })
                .PermitIf(EventType.ApproveReceived, ChallengeState.ApproveDeclined,
                    () => this.challengeService.CurrentChallenge.CurrentSuggestion.Approvals.Any(ap => !ap.isApproved))
                .PermitReentryIf(EventType.ApproveReceived,
                    () =>
                    {
                        var notAllApproved = !this.playerService.ActivePlayersNoCurrent.All(p =>
                            this.challengeService.CurrentChallenge.CurrentSuggestion.Approvals.Any(ap =>
                                ap.player.Equals(p)));
                        return notAllApproved;
                    })
                .PermitIf(EventType.PlayerLeft, ChallengeState.ChallengeUnresolved, () => !this.playerService.CurrentPlayer.IsActive)
                .OnEntry(this.OnWaitingForApprovals);

            this.challengeStateMachine.Configure(ChallengeState.ChallengeUnresolved)
                .Permit(EventType.NewChallenge, ChallengeState.NewChallenge)
                .OnEntry(this.OnChallengeUnresolved);

            this.challengeStateMachine.Configure(ChallengeState.ApproveDeclined)
                .Permit(EventType.ApproveReceived, ChallengeState.WaitingForResolution)
                .PermitIf(EventType.PlayerLeft, ChallengeState.ChallengeUnresolved, () => !this.playerService.CurrentPlayer.IsActive)
                .OnEntry(this.OnApproveDeclined);

            this.challengeStateMachine.Configure(ChallengeState.ChallengeResolved)
                .Permit(EventType.NewChallenge, ChallengeState.NewChallenge)
                .PermitIf(EventType.PlayerLeft, ChallengeState.ChallengeUnresolved, () => !this.playerService.CurrentPlayer.IsActive)
                .OnEntry(this.OnChallengeResolved);
        }

        private void OnResolutionNotValid(StateMachine<ChallengeState, EventType>.Transition transition)
        {
            this.LogTransition(transition);
            var message = $"{this.challengeService.CurrentChallenge.CurrentSuggestion.Word} is not valid";
            this.SendMessage(this.playerService.ActivePlayers, message);

            this.Resolve(EventType.ResolutionChecked);
        }

        private void OnChallengeResolved(StateMachine<ChallengeState, EventType>.Transition transition)
        {
            this.LogTransition(transition);
            this.challengeService.CurrentChallenge.Resolve();

            var message =
                $"Challenge for [{this.challengeService.CurrentChallenge.Letter}] was resolved with [{this.challengeService.CurrentChallenge.CurrentSuggestion.Word}]";
            this.SendMessage(this.playerService.ActivePlayers, message);

            this.NextTurn();
            this.communicationProxy.GameUpdated(this.challengeService.CurrentChallenge, this.playerService.CurrentPlayer, this.challengeService.Challenges);
            this.Resolve(EventType.NewChallenge);
        }

        private void OnApproveDeclined(StateMachine<ChallengeState, EventType>.Transition transition)
        {
            this.LogTransition(transition);
            var notApprovedBy = this.challengeService.CurrentChallenge.CurrentSuggestion.Approvals
                .Where(p => !p.isApproved).Select(p => $"[{p.player.Id} {p.player.Name}]");
            var message =
                $"Suggestion {this.challengeService.CurrentChallenge.CurrentSuggestion} was not approved by {string.Join(", ", notApprovedBy)}";

            this.SendMessage(this.playerService.ActivePlayers, message);

            this.Resolve(EventType.ApproveReceived);
        }

        private void OnChallengeUnresolved(StateMachine<ChallengeState, EventType>.Transition transition)
        {
            this.LogTransition(transition);
            var player = this.playerService.CurrentPlayer;
            player.SetInactive();

            var message =
                $"Player [{player.Id} {player.Name}] gave up on resolving challenge for [{this.challengeService.CurrentChallenge.Letter}] letter";

            this.SendMessage(this.playerService.ActivePlayers, message);

            this.NextTurn();
            this.communicationProxy.GameUpdated(this.challengeService.CurrentChallenge, this.playerService.CurrentPlayer, this.challengeService.Challenges);
            this.Resolve(EventType.NewChallenge);
        }

        private void OnWaitingForApprovals(StateMachine<ChallengeState, EventType>.Transition transition)
        {
            this.LogTransition(transition);
            var player = this.playerService.CurrentPlayer;

            var message =
                $"Waiting approvals for [{player.Id} {player.Name}] [{this.challengeService.CurrentChallenge.CurrentSuggestion.Word}]";

            this.SendMessage(this.playerService.ActivePlayers, message);
            this.communicationProxy.NeedApproval(this.playerService.ActivePlayersNoCurrent, this.challengeService.CurrentChallenge.CurrentSuggestion);
        }

        private void OnNewChallenge(StateMachine<ChallengeState, EventType>.Transition transition)
        {
            this.LogTransition(transition);
            var player = this.playerService.CurrentPlayer;

            var message =
                $"New challenge for [{player.Id} {player.Name}] on letter [{this.challengeService.CurrentChallenge.Letter}]";

            this.SendMessage(this.playerService.ActivePlayers, message);
            this.Resolve(EventType.NewChallenge);
        }

        private void OnWaitingForResolution(StateMachine<ChallengeState, EventType>.Transition transition)
        {
            this.LogTransition(transition);
            this.communicationProxy.NewChallenge(this.playerService.CurrentPlayer, this.challengeService.CurrentChallenge);
        }

        private void OnGameStopped(StateMachine<GameState, EventType>.Transition transition)
        {
            this.LogTransition(transition);
            this.communicationProxy.NewChallenge(this.playerService.CurrentPlayer, this.challengeService.CurrentChallenge);
            this.challengeService.Reset();
            this.playerService.Reset();
        }

        private void OnGameStarted(StateMachine<GameState, EventType>.Transition transition)
        {
            this.LogTransition(transition);
            this.InitializeChallengeStateMachine();
            this.NextTurn();
            this.Resolve(EventType.NewChallenge);
        }

        private void NextTurn()
        {
            this.playerService.NextPlayer();
            this.challengeService.NextChallengeFor(this.playerService.CurrentPlayer);
        }

        private void Resolve(EventType eventType)
        {
            this.TryFireGameState(eventType);
            this.TryFireChallengeState(eventType);
        }

        private void TryFireGameState(EventType eventType)
        {
            if (this.gameStateMachine.CanFire(eventType))
            {
                this.gameStateMachine.Fire(eventType);
            }
        }

        private void TryFireChallengeState(EventType eventType)
        {
            if (this.gameState == GameState.InProgress && this.challengeStateMachine.CanFire(eventType))
            {
                this.challengeStateMachine.Fire(eventType);
            }
            else
            {
                this.logger.LogWarning($"Ignoring event {eventType} at state {this.challengeState} with game {this.gameState}");
            }
        }

        public void OnResolutionProvided(string playerId, Suggestion suggestion)
        {
            var player = this.playerService.GetPlayer(playerId);
            if (this.playerService.CurrentPlayer.Id != player.Id)
            {
                throw new InvalidOperationException($"Expected solution from player {this.playerService.CurrentPlayer.Id} but got from {player.Id}");
            }
            this.logger.LogDebug($"Player {this.playerService.CurrentPlayer.Id} {this.playerService.CurrentPlayer.Name} provided [{suggestion.IsNotProvided}] solution {suggestion.Word}");
            this.challengeService.SuggestWithValidation(suggestion);

            this.Resolve(EventType.ResolutionProvided);
        }


        public void OnApprovalProvided(string playerId, bool isApproved)
        {
            var player = this.playerService.GetPlayer(playerId);
            this.logger.LogDebug($"Player {player.Id} {player.Name} provided approval [{isApproved}]");

            this.challengeService.AddApproval(player, isApproved);

            this.Resolve(EventType.ApproveReceived);
        }

        public void OnPlayerLeft(string playerId)
        {
            var player = this.playerService.GetPlayer(playerId);
            player.SetInactive();
            var isCurrentPlayer = player == this.playerService.CurrentPlayer;
            this.playerService.Remove(player);

            var message = $"Player {player.Id} {player.Name} left this game, is current player [{isCurrentPlayer}]";
            this.SendMessage(this.playerService.ActivePlayers, message);

            this.Resolve(EventType.PlayerLeft);
        }

        public void OnPlayerJoined(string playerId, string playerName)
        {
            var player = new Player(playerId, playerName);
            this.playerService.Add(player);

            var message = $"Player {player.Id} {player.Name} joined this game";
            this.SendMessage(this.playerService.ActivePlayers, message);

            this.Resolve(EventType.PlayerJoined);
        }

        private void LogTransition<T>(StateMachine<T, EventType>.Transition transition)
        {
            this.logger.LogDebug($"Transition from {transition.Source} to {transition.Destination} by trigger {transition.Trigger}");
        }

        private void SendMessage(List<Player> players, string message)
        {
            this.communicationProxy.Notify(players, message);
            this.logger.LogDebug($"Message [{message}] was sent");
        }
    }
}