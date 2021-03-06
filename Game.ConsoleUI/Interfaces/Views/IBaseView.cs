﻿namespace Game.ConsoleUI.Interfaces.Views
{
    using System.Collections.Generic;

    public interface IBaseView
    {
        bool WaitForConfirmation(string message);

        void ShowWarning(string message);

        List<string> WaitForInputList(string message);

        string WaitForInput(string message);

        void Refresh(string message);
    }
}