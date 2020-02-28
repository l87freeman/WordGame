namespace WordGame.Game.Domain.Interfaces
{
    using System.Threading.Tasks;
    using WordGame.Game.Domain.Models;

    public interface IGameManager
    {
        Task<Game> GetGameAsync(string userIp);

        Task<Game> CreateGameAsync(Game game);

        Task<Game> ResolveChallengeAsync(Game game);
    }
}