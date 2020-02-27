namespace WordGame.Common
{
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Events;

    public static class LoggerExtensions
    {
        public static IHostBuilder WithSerilog(this IHostBuilder builder)
        {

            builder.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .ReadFrom
                    .Configuration(hostingContext.Configuration);
            });

            return builder;
        }
    }
}
