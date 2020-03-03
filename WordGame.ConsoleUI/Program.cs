namespace WordGame.ConsoleUI
{
    using System;
    using Domain;
    using Domain.Interfaces;
    using Domain.Views;
    using Domain.Views.Interfaces;
    using Infrastructure;
    using Infrastructure.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using WordGame.Common;

    class Program
    {
        static void Main(string[] args)
        {
            InterceptExceptions();
            CreateHostBuilder().Build().Run();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            var host = new HostBuilder()
                .WithJsonConfiguration("appsettings.json")
                .ConfigureServices((hostContext, services) =>
                {
                    var configSection = hostContext.Configuration.GetSection(nameof(GameConfiguration));
                    services.Configure<GameConfiguration>(configSection);
                    services.AddSingleton<IGameManager, GameManager>();
                    services.AddSingleton<IPlayerNameProvider, PlayerNameProvider>();
                    services.AddHostedService<GameService>();
                    services.AddSingleton<IGameClient, GameClient>();
                    services.AddSingleton<IBaseView, ConsoleView>();
                    services.AddSingleton<IDispatcher, Dispatcher>();
                })
                .WithSerilog();

            return host;
        }

        private static void InterceptExceptions()
        {
            AppDomain.CurrentDomain.UnhandledException += 
                (sender, exceptionArgs) => Console.WriteLine(exceptionArgs.ExceptionObject);
        }
    }
}
