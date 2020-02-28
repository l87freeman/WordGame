namespace Game.ConsoleUI.Interfaces.Services
{
    using Game.ConsoleUI.WordGame.Models;

    public interface IBotService
    {
        string ResolveChallenge(GameChallenge challenge);
    }
}