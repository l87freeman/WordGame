namespace Game.ConsoleUI.Game.Services
{
    using Serilog;

    public abstract class BaseServiceWithLogger<T>
    {
        protected ILogger Logger { get; }

        protected BaseServiceWithLogger(ILogger logger)
        {
            this.Logger = logger.ForContext<T>();
        }
    }
}