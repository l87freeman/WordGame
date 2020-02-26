namespace Game.ConsoleUI.Interfaces.Services
{
    using Game.Models;

    public interface IGameStateService
    {
        GameState GetOrCreateGameState();
    }
}