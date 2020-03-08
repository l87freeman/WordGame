using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WordGame.BotService
{
    using System.IO;
    using Common;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .WithJsonConfiguration("appsettings.json", "appsettings.Develop.json")
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:8087");
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseStartup<Startup>();
                })
                .WithSerilog();
    }
}
