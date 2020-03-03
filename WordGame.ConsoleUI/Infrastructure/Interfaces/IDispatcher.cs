namespace WordGame.ConsoleUI.Infrastructure.Interfaces
{
    using System;

    public interface IDispatcher : IDisposable
    {
        void PlanRoutine(Action actionToPerform);
    }
}