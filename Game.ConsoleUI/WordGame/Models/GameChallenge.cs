namespace Game.ConsoleUI.WordGame.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    public class GameChallenge
    {
        [JsonProperty]
        private readonly List<string> allSuggestion = new List<string>();
        [JsonProperty]
        private int resolutionIndex = -1;

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

        [JsonProperty]
        public char ChallengeLetter { get; }

        [JsonProperty]
        public string ChallengeResolution => this.resolutionIndex < 0 ? null : this.allSuggestion[this.resolutionIndex];

        [JsonProperty]
        public List<string> HistoryOfSuggestedResolutions => 
            this.allSuggestion.ToList();

        [JsonIgnore] 
        public string CurrentSuggestedResolution { get; private set; }

        public void Suggest(string word)
        {
            if (this.CurrentSuggestedResolution != null)
            {
                this.allSuggestion.Add(this.CurrentSuggestedResolution);
            }
            
            this.CurrentSuggestedResolution = word;
        }

        public void Resolve()
        {
            this.allSuggestion.Add(this.CurrentSuggestedResolution);
            this.resolutionIndex = this.allSuggestion.Count - 1;
        }

        public override string ToString()
        {
            return $"Current challenge letter: {this.ChallengeLetter}";
        }
    }
}