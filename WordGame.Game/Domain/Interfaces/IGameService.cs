namespace WordGame.Game.Domain.Interfaces
{
    using Models.Challenges;

    public interface IGameService
    {
        void OnResolutionProvided(string playerId, Suggestion suggestion);
        void OnApprovalProvided(string playerId, bool isApproved);
        void OnPlayerJoined(string playerId, string playerName);
        void OnPlayerLeft(string playerId);
    }
}