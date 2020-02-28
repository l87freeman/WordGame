namespace Game.ConsoleUI.Interfaces.Services
{
    using Game.ConsoleUI.WordGame.Models;

    public interface IChallengeService
    {
        GameChallenge Challenge { get; }

        void Resolved();

        bool IsResolutionValid();

        void CreateChallenge();
    }
}