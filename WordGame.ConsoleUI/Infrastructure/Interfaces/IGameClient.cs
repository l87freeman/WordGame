namespace WordGame.ConsoleUI.Infrastructure.Interfaces
{
    using System;

    public interface IGameClient : IDisposable
    {
        void Start();
    }
}