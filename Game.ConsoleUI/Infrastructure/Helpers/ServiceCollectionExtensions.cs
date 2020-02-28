namespace Game.ConsoleUI.Infrastructure.Helpers
{
    using Game.ConsoleUI.Infrastructure.Interfaces;
    using Game.ConsoleUI.Interfaces;
    using Game.ConsoleUI.Interfaces.Services;
    using Game.ConsoleUI.Interfaces.Views;
    using Game.ConsoleUI.WordGame;
    using Game.ConsoleUI.WordGame.Services;
    using Game.ConsoleUI.WordGame.Views;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Serilog;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSerilogLogging(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton<ILogger>(provider => new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger());

            return serviceCollection;
        }

        public static IServiceCollection AddServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var configSection = configuration.GetSection("GameConfiguration");
            var config = configSection.Get<GameConfiguration>();

            serviceCollection
                .Configure<GameConfiguration>(configSection)
                .AddSingleton<IOptions<GameConfiguration>>(cf => config)
                .AddSingleton<IGameApplication, Application>()
                .AddSingleton<IWordProvider, FileWordProvider>()
                .AddSingleton<IWordStorage, WordStorage>()
                .AddSingleton<IGameManager, GameManager>()
                .AddSingleton<IBackupService, BackupService>()
                .AddSingleton<IBotService, BotService>()
                .AddSingleton<IValidWordVerifier, ValidWordVerifier>()
                .AddSingleton<IGameStateService, GameStateService>()
                .AddSingleton<IPlayerProvider, PlayerProvider>()
                .AddSingleton<IPlayersService, PlayersService>()
                .AddSingleton<IBaseView, ConsoleView>()
                .AddSingleton<IGameManagerView, GameManagerView>()
                .AddSingleton<IGameStateServiceView, GameStateServiceView>()
                .AddSingleton<IPlayerProviderView, PlayerProviderView>()
                .AddSingleton<IPlayersServiceView, PlayersServiceView>();

            return serviceCollection;
        }
    }
}