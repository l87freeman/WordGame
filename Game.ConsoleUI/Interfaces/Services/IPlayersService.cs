namespace Game.ConsoleUI.Interfaces.Services
{
    using Game.Models;

    public interface IPlayersService
    {
        //void NextPlayer();

        bool ApproveResolution(GameChallenge challenge);

        void ResolveChallenge(GameChallenge challenge);
    }
}