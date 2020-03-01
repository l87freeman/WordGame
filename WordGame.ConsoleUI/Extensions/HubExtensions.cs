namespace WordGame.ConsoleUI.Extensions
{
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.DependencyInjection;

    public static class HubExtensions
    {
        public static void AddHubClient(this IServiceCollection serviceCollection, string hubAddress)
        {
            var connection = new HubConnectionBuilder()
                .WithAutomaticReconnect()
                .WithUrl(hubAddress)
                .Build();
            serviceCollection.AddSingleton<HubConnection>(connection);
        }
    }
}