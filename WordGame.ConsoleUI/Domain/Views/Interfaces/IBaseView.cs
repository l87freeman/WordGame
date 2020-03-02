﻿namespace WordGame.ConsoleUI.Domain.Views.Interfaces
{
    using System;

    public interface IBaseView
    {
        event EventHandler<EventArgs> BotInteractionChanged;

        bool WaitForConfirmation(string message);

        void ShowWarning(string message);

        string WaitForInput(string message);

        void Refresh(string message);

        void Display(string message);
    }
}