namespace WordGame.Game.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Interfaces;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Models.Challenges;

    public class ChallengeResolutionValidator : IChallengeResolutionValidator
    {
        private readonly ILogger<ChallengeResolutionValidator> logger;
        private readonly IDictionaryProvider dictionary;

        public ChallengeResolutionValidator(ILogger<ChallengeResolutionValidator> logger, IDictionaryProvider dictionary)
        {
            this.logger = logger;
            this.dictionary = dictionary;
        }

        public void CheckResolution(Suggestion resolutionCandidate, List<Suggestion> history)
        {
            resolutionCandidate.IsValid = this.IsNotMetInHistory(resolutionCandidate, history)
                                                            && this.IsExistsInDictionary(resolutionCandidate);

            this.logger.LogDebug($"{resolutionCandidate} is valid {resolutionCandidate.IsValid}");
        }

        private bool IsExistsInDictionary(Suggestion suggestion)
        {
            var isExists = this.dictionary.IsWordExists(suggestion.Word);
            this.logger.LogDebug($"{suggestion.Word} is exists in dictionary {isExists}");
            return isExists;
        }

        private bool IsNotMetInHistory(Suggestion suggestion, List<Suggestion> suggestions)
        {
            var isUnique = suggestions.All(s =>
                !string.Equals(suggestion.Word, s.Word, StringComparison.InvariantCultureIgnoreCase));
            this.logger.LogDebug($"{suggestion} is not met in history {isUnique}");

            return isUnique;
        }
    }
}