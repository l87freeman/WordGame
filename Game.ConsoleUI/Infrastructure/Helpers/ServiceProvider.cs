namespace Game.ConsoleUI.Infrastructure.Helpers
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceLocator
    {
        public static ServiceProvider Provider { get; set; }
    }
}