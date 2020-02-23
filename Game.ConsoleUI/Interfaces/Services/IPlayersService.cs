namespace Game.ConsoleUI.Interfaces.Services
{
    using Game.Models;

    public interface IPlayersService
    {
        void Shift();

        bool ApproveResolution(GameChallenge challenge);

        void ResolveChallenge(GameChallenge challenge);
    }
}