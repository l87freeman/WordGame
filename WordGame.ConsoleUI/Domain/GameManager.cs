namespace WordGame.ConsoleUI.Domain
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;
    using Models;
    using WordGame.ConsoleUI.Domain.Views.Interfaces;
    using WordGame.ConsoleUI.Infrastructure;
    using WordGame.ConsoleUI.Infrastructure.Interfaces;

    public class GameManager : IGameManager
    {
        private readonly IBaseView baseView;

        private readonly IDispatcher dispatcher;

        public event EventHandler<Suggestion> Resolved;

        public event EventHandler<bool> Approved;

        public GameManager(IBaseView baseView, IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            this.baseView = baseView;
        }

        public void RefreshUi(Game game)
        {
            this.dispatcher.PlanRoutine(() =>
            {
                this.baseView.Refresh(game.ToString());
            });
        }

        public void Approve(Suggestion suggestion)
        {
            this.dispatcher.PlanRoutine(() =>
            {
                var approved = this.baseView.WaitForConfirmation(suggestion.ToString());
                this.Approved?.Invoke(this, approved);
            });
        }

        public void Resolve(Challenge challenge)
        {
            this.dispatcher.PlanRoutine(() =>
            {
                var suggestion = this.GetResolution(challenge);
                if (suggestion.IsNotProvided)
                {
                    Environment.Exit(0);
                }
                this.Resolved?.Invoke(this, suggestion);
            });
        }

        public void Display(string message)
        {
            this.dispatcher.PlanRoutine(() =>
            {
                this.baseView.Display(message);
            });
        }

        private Suggestion GetResolution(Challenge challenge)
        {
            var suggestion = new Suggestion();
            if (!this.TryToResolveChallenge(challenge, out var resolution))
            {
                this.baseView.ShowWarning("You gave up on this game, bye-bye");
                suggestion.IsNotProvided = true;
            }

            suggestion.Word = resolution;

            return suggestion;
        }

        private bool TryToResolveChallenge(Challenge challenge, out string resolution)
        {
            bool inGame = true;

            resolution = this.baseView.WaitForInput($"Please provide word starting on a [{challenge.Letter}] letter or enter empty string to give up");
            if (string.IsNullOrWhiteSpace(resolution))
            {
                inGame = !this.baseView.WaitForConfirmation("Do you want to give up?");
            }

            return inGame;
        }
    }
}