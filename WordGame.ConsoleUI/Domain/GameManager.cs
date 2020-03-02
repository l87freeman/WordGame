namespace WordGame.ConsoleUI.Domain
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;
    using Models;
    using WordGame.ConsoleUI.Domain.Views.Interfaces;
    using WordGame.ConsoleUI.Infrastructure;

    public class GameManager : IGameManager
    {
        private readonly IBaseView baseView;

        private readonly Dispatcher dispatcher = new Dispatcher();

        public event EventHandler<string> Resolved;

        public event EventHandler<bool> Approved;

        public event EventHandler<EventArgs> BotInteractionChanged;

        public GameManager(IBaseView baseView)
        {
            this.baseView = baseView;
            this.ListenToOrRemoveBot();
        }

        private void ListenToOrRemoveBot()
        {
            this.baseView.BotInteractionChanged += (s, e) => { Task.Run(() =>
                {
                    this.BotInteractionChanged?.Invoke(this, EventArgs.Empty);
                }); };
        }

        public void RefreshUi(Game game)
        {
            this.dispatcher.Invoke(() =>
            {
                this.baseView.Refresh(game.ToString());
            });
        }

        public void Approve(Suggestion suggestion)
        {
            this.dispatcher.Invoke(() =>
            {
                var approved = this.baseView.WaitForConfirmation(suggestion.ToString());
                this.Approved?.Invoke(this, approved);
            });
        }

        public void Resolve(Challenge challenge)
        {
            this.dispatcher.Invoke(() =>
            {
                var suggestion = this.GetResolution(challenge);
                this.Resolved?.Invoke(this, suggestion);
            });
        }

        public void Display(string message)
        {
            this.dispatcher.Invoke(() =>
            {
                this.baseView.Display(message);
            });
            
        }

        private string GetResolution(Challenge challenge)
        {
            if (!this.TryToResolveChallenge(challenge, out var resolution))
            {
                this.baseView.ShowWarning("You gave up on this game");
                resolution = string.Empty;
            }

            return resolution;
        }

        private bool TryToResolveChallenge(Challenge challenge, out string resolution)
        {
            bool isGiveUp = false;
            do
            {
                resolution = this.baseView.WaitForInput($"Please provide word starting on a {challenge.Letter} or enter empty string to give up");
                if (string.IsNullOrWhiteSpace(resolution))
                {
                    isGiveUp = this.baseView.WaitForConfirmation("Do you want to give up?");
                }
            } while (string.IsNullOrWhiteSpace(resolution) || isGiveUp);

            return !isGiveUp;
        }
    }
}