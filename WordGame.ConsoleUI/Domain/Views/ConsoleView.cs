namespace WordGame.ConsoleUI.Domain.Views
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces;

    public class ConsoleView : IBaseView
    {
        private const string ConfirmSign = "Y";
        private const string DeclineSign = "N";

        public ConsoleView()
        {
            Console.CancelKeyPress += this.BotInteraction;
        }

        private void BotInteraction(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            if (this.WaitForConfirmation("Do you wand to add/remove a bot?"))
            {
                this.BotInteractionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> BotInteractionChanged;

        public void Refresh(string linesToDisplay)
        {
            Console.Clear();
            Console.Write(linesToDisplay);
            this.DisplayMessageInColor("To add a bot click Ctrl + C", ConsoleColor.DarkYellow);
        }

        public void Display(string message)
        {
            this.DisplayMessageInColor(message, ConsoleColor.DarkBlue);
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
            var initialColor = Console.BackgroundColor;
            Console.BackgroundColor = color;
            Console.WriteLine(message);
            Console.BackgroundColor = initialColor;
        }

        private bool TryParseResult(string stringToCheck, out bool conformed)
        {
            conformed = string.Equals(stringToCheck, ConfirmSign, StringComparison.InvariantCultureIgnoreCase);

            return conformed || string.Equals(stringToCheck, DeclineSign, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}