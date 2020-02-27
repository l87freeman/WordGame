namespace WordGame.Common
{
    using System;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Events;

    public static class LoggerExtensions
    {
        public const string BaseDirectoryVariableName = "BASEDIR";

        public static IHostBuilder WithSerilog(this IHostBuilder builder)
        {
            SetBaseDirIfNotExists();

            builder.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .ReadFrom
                    .Configuration(hostingContext.Configuration);
            });

            return builder;

            void SetBaseDirIfNotExists()
            {
                var baseDir = Environment.GetEnvironmentVariable(BaseDirectoryVariableName);
                if (baseDir == null)
                {
                    Environment.SetEnvironmentVariable(BaseDirectoryVariableName, AppDomain.CurrentDomain.BaseDirectory);
                }
            }
        }
    }
}
