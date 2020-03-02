namespace WordGame.ConsoleUI.Domain.Interfaces
{
    using System;
    using Models;

    public interface IGameManager
    {
        event EventHandler<bool> Approved;

        event EventHandler<string> Resolved;

        event EventHandler<EventArgs> BotInteractionChanged;

        void RefreshUi(Game game);

        void Approve(Suggestion suggestion);

        void Resolve(Challenge challenge);

        void Display(string message);
    }
}