namespace WordGame.ConsoleUI
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNet.SignalR.Client;
    using Microsoft.Extensions.Hosting;

    public class GameClient : IHostedService
    {
        private readonly IConnectionClient connectionClient;

        public GameClient(IConnectionClient connectionClient)
        {
            this.connectionClient = connectionClient;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}