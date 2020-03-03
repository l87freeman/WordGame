namespace WordGame.ConsoleUI.Infrastructure
{
    using System;
    using System.Collections.Concurrent;
    using System.Timers;
    using Microsoft.Extensions.Logging;
    using WordGame.ConsoleUI.Infrastructure.Interfaces;
    using Timer = System.Timers.Timer;

    public class Dispatcher : IDispatcher
    {
        private readonly ILogger<Dispatcher> logger;
        private readonly ConcurrentQueue<Action> tasksToPerform = new ConcurrentQueue<Action>();
        private readonly Timer timer = new Timer(50);

        public Dispatcher(ILogger<Dispatcher> logger)
        {
            this.logger = logger;
            this.timer.Elapsed += this.RunTasks;
            this.timer.Start();
        }

        private void RunTasks(object sender, ElapsedEventArgs e)
        {
            if (this.tasksToPerform.TryDequeue(out var taskToRun))
            {
                taskToRun();
            }
        }

        public void PlanRoutine(Action actionToPerform)
        {
            Action task = () =>
            {
                try
                {
                    actionToPerform();
                }
                catch (Exception e)
                {
                    this.logger.LogError(e, "Exception occured while performing routine");
                }
            };
            this.tasksToPerform.Enqueue(task);
        }

        public void Dispose()
        {
            this.timer.Stop();
            this.timer.Dispose();
        }
    }
}