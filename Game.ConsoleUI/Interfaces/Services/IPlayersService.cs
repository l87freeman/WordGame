namespace Game.ConsoleUI.Interfaces.Services
{
    using Game.ConsoleUI.WordGame.Models;

    public interface IPlayersService
    {
        //void NextPlayer();

        bool ApproveResolution(GameChallenge challenge);

        void ResolveChallenge(GameChallenge challenge);
    }
}