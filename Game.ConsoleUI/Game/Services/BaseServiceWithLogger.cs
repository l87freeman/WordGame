namespace Game.ConsoleUI.Game.Services
{
    using global::Game.ConsoleUI.Infrastructure.Helpers;
    using Serilog;

    public abstract class BaseServiceWithLogger<T>
    {
        protected ILogger Logger { get; }

        protected BaseServiceWithLogger(ILogger logger)
        {
            ExceptionHelpers.ThrowOnNullArgument(nameof(logger), logger);

            this.Logger = logger.ForContext<T>();
        }
    }
}