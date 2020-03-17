namespace WordGame.Game.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Configuration;
    using Domain.Interfaces;
    using Domain.Models.Players;
    using Dto;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Challenge = Domain.Models.Challenges.Challenge;

    public class BotServiceRemote : IBotService
    {
        private readonly ILogger<BotServiceRemote> logger;
        private readonly IPlayerService playerService;
        private readonly IChallengeService challengeService;
        private Action<string, Suggestion> onResolutionProvided;
        private Action<string, string> onApprovalProvided;
        private readonly string address;

        public BotServiceRemote(ILogger<BotServiceRemote> logger, IOptions<BotConfiguration> options, IPlayerService playerService, IChallengeService challengeService)
        {
            this.logger = logger;
            this.playerService = playerService;
            this.challengeService = challengeService;
            this.address = options.Value.Address;
        }

        public void OnResolutionProvided(Action<string, Suggestion> callback)
        {
            this.onResolutionProvided = callback;
        }

        public void OnApprovalProvided(Action<string, string> callback)
        {
            this.onApprovalProvided = callback;

        }

        public void NewChallenge()
        {
            if (this.playerService.IsGameWithBot && this.playerService.CurrentPlayer is BotPlayer)
            {
                var resolution = this.Resolve(this.challengeService.CurrentChallenge, this.challengeService.Challenges);
                this.onResolutionProvided(BotPlayer.BotPlayerId, resolution);
            }
        }

        public void NeedApproval(Suggestion suggestion)
        {
            if (this.playerService.IsGameWithBot && !(this.playerService.CurrentPlayer is BotPlayer))
            {
                this.onApprovalProvided(BotPlayer.BotPlayerId, true.ToString());
            }
        }

        private Suggestion Resolve(Challenge challenge, List<Challenge> challengesHistory)
        {
            var used = challengesHistory.SelectMany(c => c.Suggestions).Select(c => c.Word).ToList();
            used.AddRange(challenge.Suggestions.Select(s => s.Word));

            var suggestion = this.GetResolutionAsync(challenge.Letter, used).GetAwaiter().GetResult();

            return suggestion;
        }

        private async Task<Suggestion> GetResolutionAsync(char challengeLetter, List<string> challengesHistory)
        {
            var dto = new BotChallenge
            {
                Suggestions = challengesHistory.Select(c => new Dto.Suggestion {Word = c}).ToList(),
                Challenge = challengeLetter
            };
            var serString = await this.ReadFromRemoteAsync(dto);
            var result = this.Convert(serString);

            return result;
        }

        private async Task<string> ReadFromRemoteAsync(BotChallenge dto)
        {
            string resultString = string.Empty;
            using (var httpClient = new HttpClient(new HttpClientHandler()))
            {
                try
                {
                    var requestMessage = new HttpRequestMessage(HttpMethod.Get, this.address)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8,
                            "application/json")
                    };
                    var response = await httpClient.SendAsync(requestMessage);
                    if (response.IsSuccessStatusCode)
                    {
                        resultString = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        this.logger.LogError($"Error code [{response.StatusCode}] on attempt to reach remote address [{this.address}]");
                    }

                }
                catch (Exception e)
                {
                    this.logger.LogError(e, "Was not able to read from remote");
                }

            }
            return resultString;
        }

        private Dto.Suggestion Convert(string resultString)
        {
            Dto.Suggestion result = new Dto.Suggestion { IsNotProvided = true };
            try
            {
                result = JsonConvert.DeserializeObject<Suggestion>(resultString);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"Was not able to DeserializeObject from string {resultString}");
            }

            return result;
        }
    }
}