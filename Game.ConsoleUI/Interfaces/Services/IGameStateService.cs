namespace Game.ConsoleUI.Interfaces.Services
{
    using System.Collections.Generic;
    using Game.Models;

    public interface IGameStateService
    {
        GameState GetOrCreateGameState();

        List<GamePlayer> GetPlayers();

        GamePlayer NextPlayer();
    }
}