namespace WordGame.ConsoleUI.Domain.Views
{
    using System;
    using Interfaces;

    public class ConsoleView : IBaseView
    {
        private const string ConfirmSign = "Y";
        private const string DeclineSign = "N";

        public void Refresh(string linesToDisplay)
        {
            Console.Clear();
            this.DisplayMessageInColor(linesToDisplay, ConsoleColor.DarkYellow);
        }

        public void Display(string message)
        {
            this.DisplayMessageInColor(message, ConsoleColor.DarkMagenta);
        }

        public string WaitForInput(string displayMessage)
        {
            this.DisplayMessageInColor($"{displayMessage} : ", ConsoleColor.DarkGreen);
            var input = Console.ReadLine();

            return input;
        }

        public bool WaitForConfirmation(string displayMessage)
        {
            displayMessage = $"{displayMessage}{Environment.NewLine}To confirm press {ConfirmSign}, to decline press {DeclineSign} and hit enter";

            string resultString = null;
            bool conformed;
            while (!this.TryParseResult(resultString, out conformed))
            {
                resultString = this.WaitForInput(displayMessage);
            }

            return conformed;
        }

        public void ShowWarning(string message)
        {
            this.DisplayMessageInColor(message, ConsoleColor.Red);
        }

        private void DisplayMessageInColor(string message, ConsoleColor color)
        {
            lock (this)
            {
                var initialColor = Console.BackgroundColor;
                Console.BackgroundColor = color;
                Console.WriteLine(message);
                Console.BackgroundColor = initialColor;
            }
        }

        private bool TryParseResult(string stringToCheck, out bool conformed)
        {
            conformed = string.Equals(stringToCheck, ConfirmSign, StringComparison.InvariantCultureIgnoreCase);

            return conformed || string.Equals(stringToCheck, DeclineSign, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}