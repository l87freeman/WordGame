namespace Game.ConsoleUI.Interfaces.Services
{
    using System.Collections.Generic;
    using global::Game.ConsoleUI.Game.Models;

    public interface IValidWordVerifier
    {
        bool IsValid(List<GameChallenge> history, string resolution);
    }
}