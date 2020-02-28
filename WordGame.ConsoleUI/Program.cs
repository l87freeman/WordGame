namespace WordGame.ConsoleUI
{
    using System;
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
                .ConfigureServices((hostContext, services) => { services.AddHostedService<GameClient>(); })
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
