namespace Game.ConsoleUI.Game.Services
{
    using System;
    using System.Linq;
    using global::Game.ConsoleUI.Interfaces.Services;
    using Infrastructure.Interfaces;
    using Serilog;

    public class ValidWordVerifier : IValidWordVerifier
    {
        private readonly ILogger logger;
        private readonly IWordStorage wordStorage;

        public ValidWordVerifier(ILogger logger, IWordStorage wordStorage)
        {
            this.logger = logger.ForContext<ValidWordVerifier>();
            this.wordStorage = wordStorage;
        }

        public bool IsValid(string challenge)
        {
            var words = this.wordStorage.GetWords(challenge[0]);
            var isWordFeasible = words.Any(word =>
                string.Equals(word, challenge, StringComparison.InvariantCultureIgnoreCase));
            return isWordFeasible;
        }
    }
}