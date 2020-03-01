namespace WordGame.ConsoleUI.Domain
{
    using System;
    using Interfaces;
    using Models;

    public class GameManager : IGameManager
    {
        public event EventHandler<Challenge> Resolved;

        public event EventHandler<bool> Approved;

        public void RefreshUi(Game game)
        {
            throw new NotImplementedException();
        }

        public void Approve(Suggestion suggestion)
        {
            throw new NotImplementedException();
        }

        public void Resolve(Challenge challenge)
        {
            throw new NotImplementedException();
        }
    }
}