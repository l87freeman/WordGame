namespace WordGame.Game.Domain.Interfaces
{
    using System.Threading.Tasks;
    using WordGame.Game.Domain.Models;

    public interface IGameManager
    {
        Task ApplyApprovalAsync(PlayerInfo player, bool isApproved);

        Task ApplyResolutionAsync(PlayerInfo player, string message);
        
        void PlayerJoined(PlayerInfo player);

        void PlayerLeft(PlayerInfo player);
    }
}