using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WordGame.GameState
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
                .WithJsonConfiguration("appsettings.json", "appsettings.Development.json")
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://localhost:8088");
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseStartup<Startup>();
                })
                .WithSerilog();
    }
}
