namespace Game.ConsoleUI.Game.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class GameChallenge
    {
        public GameChallenge(char challengeLetter)
        {
            ChallengeLetter = challengeLetter;
        }

        public char ChallengeLetter { get; }

        public string ChallengeResolution { get; private set; }

        [JsonIgnore]
        public string SuggestedResolution { get; private set; }

        public HashSet<string> SuggestedResolutions { get; } = new HashSet<string>();

        public void Suggest(string word)
        {
            this.SuggestedResolution = word;
            this.SuggestedResolutions.Add(word);
        }

        public void Resolve(string word)
        {
            this.ChallengeResolution = word;
        }

        public override string ToString()
        {
            var tail = this.ChallengeResolution == null
                ? $"word suggested: {this.SuggestedResolution}"
                : "word provided: {providedWord}";
            return $"Current letter: {this.ChallengeLetter}, {tail}";
        }
    }
}