namespace WordGame.ConsoleUI.Domain.Interfaces
{
    using System;
    using Models;

    public interface IGameManager
    {
        event EventHandler<bool> Approved;

        event EventHandler<Challenge> Resolved;

        void RefreshUi(Game game);

        void Approve(Suggestion suggestion);

        void Resolve(Challenge challenge);
    }
}