namespace WordGame.Game.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Models.Challenges;
    using Models.Players;

    public class MessageProvider : IMessageProvider
    {
        private readonly Dictionary<Enum, Func<PlayerEventData, string>> playersMessageBuilders = new Dictionary<Enum, Func<PlayerEventData, string>>();
        private readonly Dictionary<Enum, Func<ChallengeEventData, string>> challengeMessageBuilders = new Dictionary<Enum, Func<ChallengeEventData, string>>();

        public MessageProvider()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.playersMessageBuilders[PlayerEventType.PlayerJoined] = data => $"{data.EventByPlayer} joined this game, {data.PlayersCount} players in game";
            this.playersMessageBuilders[PlayerEventType.PlayerLeft] = data => $"{data.EventByPlayer} left this game, {data.PlayersCount} players in game";
            this.playersMessageBuilders[PlayerEventType.NextPlayer] = data => $"{data.EventByPlayer} is next";

            this.challengeMessageBuilders[ChallengeEventType.New] = data => $"New {data.EventByChallenge}";
            this.challengeMessageBuilders[ChallengeEventType.Suggestion] = data => $"Resolution received: {data.EventByChallenge}";
            this.challengeMessageBuilders[ChallengeEventType.Validation] = data => $"Validation performed {data.EventByChallenge}";
            this.challengeMessageBuilders[ChallengeEventType.Approve] = data =>
            {
                var approvals = string.Join("",
                    data.Approvals.Select(kv => $"{Environment.NewLine}{kv.Key} - {kv.Value}"));
                return
                        $"{data.EventByChallenge} with approvals {approvals}";
            };
            this.challengeMessageBuilders[ChallengeEventType.Resolved] = data => $"{data.EventByChallenge} RESOLVED";
        }

        public string GetMessage(PlayerEventData eventData)
        {
            var message = this.playersMessageBuilders[eventData.EventType](eventData);
            return message;
        }

        public string GetMessage(ChallengeEventData eventData)
        {
            var message = this.challengeMessageBuilders[eventData.EventType](eventData);
            return message;
        }
    }
}
