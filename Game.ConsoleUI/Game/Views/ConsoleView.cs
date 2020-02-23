namespace Game.ConsoleUI.Game.Views
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Views;

    public class ConsoleView : IBaseView
    {
        private const string ConfirmSign = "Y";
        private const string DeclineSign = "N";

        public void Refresh(string linesToDisplay)
        {
            Console.Clear();
            Console.Write(linesToDisplay);
        }

        public string WaitForInput(string displayMessage)
        {
            string input = null;
            while (string.IsNullOrWhiteSpace(input))
            {
                this.DisplayMessageInColor($"{displayMessage} : ", ConsoleColor.DarkGreen);
                input = Console.ReadLine();
            }

            return input;
        }

        public List<string> WaitForInputList(string displayMessage)
        {
            var inputList = new List<string>();
            var inputFinished = false;
            displayMessage = $"{displayMessage}, or enter {DeclineSign} to stop";
            do
            {
                var entered = this.WaitForInput(displayMessage);
                if (string.Equals(entered, DeclineSign, StringComparison.InvariantCultureIgnoreCase))
                {
                    inputFinished = true;
                }
                else
                {
                    inputList.Add(entered);
                }
            } while (!inputFinished);
            
            return inputList;
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