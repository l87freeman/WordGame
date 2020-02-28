namespace Game.ConsoleUI.Interfaces.Services
{
    using Game.ConsoleUI.WordGame.Models;

    public interface IGameStateService
    {
        GameState GetOrCreateGameState();
    }
}