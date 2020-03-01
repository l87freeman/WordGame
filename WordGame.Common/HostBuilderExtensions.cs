namespace WordGame.Common
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Events;

    public static class HostBuilderExtensions
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

        public static IHostBuilder WithJsonConfiguration(this IHostBuilder builder, params string[] configFiles)
        {
            if (configFiles.Length == 0)
            {
                throw new InvalidOperationException("No configuration files where provided");
            }

            builder.ConfigureHostConfiguration(b =>
            {
                b.AddJsonFile(configFiles[0]);
                for (int fileIndex = 1; fileIndex < configFiles.Length - 1; fileIndex++)
                {
                    b.AddJsonFile(configFiles[fileIndex], optional: true);
                }
            });

            return builder;
        }
    }
}
