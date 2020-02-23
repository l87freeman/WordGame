namespace Game.ConsoleUI
{
    using System;
    using Infrastructure.Helpers;
    using Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    class Program
    {

        static void Main()
        {
            ConfigureServices();
            AddUnhandledErrorListener();

            Run();
        }

        private static void Run()
        {
            var app = ServiceLocator.Provider.GetRequiredService<IGameApplication>();
            app.Run();
        }

        private static void AddUnhandledErrorListener()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var logger = ServiceLocator.Provider.GetRequiredService<ILogger>();
                logger.Error(e.ExceptionObject as Exception, "Unhandled error occured");
            };
        }

        private static void ConfigureServices()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", true)
                .Build();

            ServiceLocator.Provider = new ServiceCollection()
                .AddSerilogLogging(configuration)
                .AddServices(configuration)
                .BuildServiceProvider();
        }
    }
}
