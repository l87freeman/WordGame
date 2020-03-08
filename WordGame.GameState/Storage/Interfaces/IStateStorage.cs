namespace WordGame.GameState.Storage.Interfaces
{
    using System.Threading.Tasks;
    using Models;

    public interface IStateStorage
    {
        Task SaveAsync(GameDto state);
    }
}