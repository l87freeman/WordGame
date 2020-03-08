namespace WordGame.Game.Dto
{
    using Newtonsoft.Json;

    public class PlayerInfo
    {
        [JsonConstructor]
        private PlayerInfo()
        {
        }

        public PlayerInfo(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        [JsonProperty]
        public string Name { get; }

        [JsonProperty]
        public string Id { get; }

        [JsonProperty]
        public bool IsActive { get; set; }
    }
}