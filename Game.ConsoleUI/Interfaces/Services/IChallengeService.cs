namespace Game.ConsoleUI.Interfaces.Services
{
    using Game.Models;

    public interface IChallengeService
    {
        GameChallenge Challenge { get; }

        void Resolved();

        bool IsResolutionValid();

        void CreateChallenge();
    }
}