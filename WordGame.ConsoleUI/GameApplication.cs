namespace WordGame.ConsoleUI
{
    using System.Threading;
    using System.Threading.Tasks;
    using Infrastructure.Interfaces;
    using Microsoft.Extensions.Hosting;

    public class GameApplication : IHostedService
    {
        private readonly IGameClient gameClient;

        public GameApplication(IGameClient gameClient)
        {
            this.gameClient = gameClient;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.gameClient.Start();
            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.gameClient.Dispose();
            return Task.CompletedTask;
        }
    }
}