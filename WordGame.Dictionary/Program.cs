using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WordGame.Dictionary
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
                    webBuilder.UseUrls("http://0.0.0.0:8086", "http://0.0.0.0:8080", "http://0.0.0.0:80");
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseStartup<Startup>();
                })
                .WithSerilog();
    }
}
