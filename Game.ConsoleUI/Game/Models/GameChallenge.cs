namespace Game.ConsoleUI.Game.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class GameChallenge
    {
        public GameChallenge(char challengeLetter)
        {
            this.Id = Guid.NewGuid().ToString();
            this.ChallengeLetter = challengeLetter;
        }

        [JsonConstructor]
        public GameChallenge(string id, char challengeLetter)
        {
            this.Id = id;
            this.ChallengeLetter = challengeLetter;
        }

        [JsonProperty]
        public string Id { get; private set; }

        public char ChallengeLetter { get; }

        [JsonProperty]
        public string ChallengeResolution { get; private set; }

        public HashSet<string> SuggestedResolutions { get; } = new HashSet<string>();

        [JsonIgnore]
        public string SuggestedResolution { get; private set; }

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