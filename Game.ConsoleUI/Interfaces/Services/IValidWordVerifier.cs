namespace Game.ConsoleUI.Interfaces.Services
{
    using System.Collections.Generic;
    using Game.ConsoleUI.WordGame.Models;

    public interface IValidWordVerifier
    {
        bool IsValid(List<GameChallenge> history, string resolution);
    }
}