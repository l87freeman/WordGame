namespace WordGame.ConsoleUI.Infrastructure
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Timers;
    using Timer = System.Timers.Timer;

    public class Dispatcher : IDisposable
    {
        private readonly ConcurrentQueue<Task> tasksToPerform = new ConcurrentQueue<Task>();
        private readonly ManualResetEventSlim resetEvent = new ManualResetEventSlim(true);
        private readonly Timer timer = new Timer(50);

        public Dispatcher()
        {
            this.timer.Elapsed += this.RunTasks;
            this.timer.Start();
        }

        private void RunTasks(object sender, ElapsedEventArgs e)
        {
            if (this.tasksToPerform.TryDequeue(out var taskToRun))
            {
                taskToRun.Start();
            }
        }

        public void PlanRoutine(Action actionToPerform)
        {
            var task = new Task(() =>
            {
                try
                {
                    this.resetEvent.Wait();
                    this.resetEvent.Reset();
                    actionToPerform();
                }
                finally
                {
                    this.resetEvent.Set();
                }
            });
            this.tasksToPerform.Enqueue(task);
        }

        public void Dispose()
        {
            this.timer.Stop();
            this.timer.Dispose();
        }
    }
}