namespace Game.ConsoleUI.Interfaces.Services
{
    using Game.Models;

    public interface IBotService
    {
        string ResolveChallenge(GameChallenge challenge);
    }
}