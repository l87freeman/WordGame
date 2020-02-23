namespace Game.ConsoleUI.Interfaces.Services
{
    using Game.Models;

    public interface IBackupService
    {
        bool TryRestoreGame(out GameState gameState);

        bool TryStoreGame(GameState gameState);
    }
}