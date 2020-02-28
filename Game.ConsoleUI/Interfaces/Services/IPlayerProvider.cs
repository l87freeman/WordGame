namespace Game.ConsoleUI.Interfaces.Services
{
    using System.Collections.Generic;
    using Game.ConsoleUI.WordGame.Models;

    public interface IPlayerProvider
    {
        List<GamePlayer> GetPlayers();
    }
}