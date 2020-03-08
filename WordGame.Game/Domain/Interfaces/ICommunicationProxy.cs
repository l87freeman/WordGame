namespace WordGame.Game.Domain.Interfaces
{
    using System.Collections.Generic;
    using Models.Challenges;
    using Models.Players;

    public interface ICommunicationProxy
    {
        void Notify(List<Player> players, string message);

        void NewChallenge(Player player, Challenge challenge);

        void NeedApproval(List<Player> players, Suggestion suggestion);

        void GameUpdated(Challenge currentChallenge, Player player, List<Challenge> challenges);
        
        void OnApprovalProvided(string playerId, string isApproved);
        
        void OnResolutionProvided(string playerId, Dto.Suggestion suggestion);
        
        void OnPlayerJoined(string playerId, string playerName);
        
        void OnPlayerLeft(string playerId);
    }
}