namespace WordGame.Game.Domain.Interfaces
{
    using System.Threading.Tasks;
    using WordGame.Game.Domain.Models;

    public interface IGameStateProvider
    {
        Task<Game> TryRestoreGame(string gameId);

        Task<Game> GetOrCreate(Game game);

        Task<Game> SynchronizeAsync(Game game);
    }
}