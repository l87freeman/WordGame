namespace WordGame.Game.Domain.Interfaces
{
    using Models.Players;

    public interface IGameManager
    {
        void ApplyApproval(PlayerInfo player, bool isApproved);

        void ApplyResolution(PlayerInfo player, string suggestion);
        
        void PlayerJoined(PlayerInfo player);

        void PlayerLeft(PlayerInfo player);
    }
}