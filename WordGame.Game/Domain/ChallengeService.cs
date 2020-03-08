namespace WordGame.Game.Domain
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Models;
    using Models.Challenges;
    using Models.Players;

    public class ChallengeService : IChallengeService
    {
        private const string alpha = "abcdefghijklmnopqrstuvwxyz";

        private readonly ILogger<ChallengeService> logger;
        private readonly IChallengeResolutionValidator challengeResolutionValidator;
        private readonly List<Challenge> challengesHistory = new List<Challenge>();

        public ChallengeService(ILogger<ChallengeService> logger, IChallengeResolutionValidator challengeResolutionValidator)
        {
            this.logger = logger;
            this.challengeResolutionValidator = challengeResolutionValidator;
        }

        public Challenge CurrentChallenge { get; private set; }

        public List<Challenge> Challenges => this.challengesHistory.ToList();

        public void SuggestWithValidation(Suggestion suggestion)
        {
            var suggestionHistory = this.GetSuggestionHistory();
            this.CurrentChallenge.Suggest(suggestion);

            this.challengeResolutionValidator.CheckResolution(this.CurrentChallenge.CurrentSuggestion, suggestionHistory);
        }

        private List<Suggestion> GetSuggestionHistory()
        {
            var currentChallengeSuggestions = this.CurrentChallenge.Suggestions.ToList();
            var challengesSuggestions = this.challengesHistory.Where(ch => ch != this.CurrentChallenge)
                .SelectMany(ch => ch.Suggestions).ToList();
            challengesSuggestions.AddRange(currentChallengeSuggestions);

            return challengesSuggestions;
        }

        public void NextChallengeFor(Player player)
        {
            this.CreateChallenge(player);
            this.challengesHistory.Add(this.CurrentChallenge);
            this.logger.LogDebug($"Challenge challenge was created {this.CurrentChallenge} for {player.Id} {player.Name}");
        }

        public void AddApproval(Player player, bool isApproved)
        {
            lock (this)
            {
                this.CurrentChallenge.AddApproval(player, isApproved);
            }
            this.logger.LogDebug($"Approval [{isApproved}] from {player.Id} {player.Name} was added to approvals");
        }

        public void Reset()
        {
            this.CurrentChallenge = null;
            this.challengesHistory.Clear();
        }

        private void CreateChallenge(Player player)
        {
            var nextChallengeLetter = this.CurrentChallenge?.CurrentSuggestion?.Word?.ToCharArray().Last() ?? this.InitialChallenge();
            this.CurrentChallenge = new Challenge(nextChallengeLetter, player);
        }

        private char InitialChallenge()
        {
            var charsToSelect = alpha.ToCharArray();
            var selectedCharIndex = new Random().Next(0, charsToSelect.Length - 1);

            return charsToSelect[selectedCharIndex];
        }
    }
}