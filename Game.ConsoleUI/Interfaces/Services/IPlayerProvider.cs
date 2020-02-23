namespace Game.ConsoleUI.Interfaces.Services
{
    using System.Collections.Generic;
    using Game.Models;

    public interface IPlayerProvider
    {
        List<GamePlayer> GetPlayers();
    }
}