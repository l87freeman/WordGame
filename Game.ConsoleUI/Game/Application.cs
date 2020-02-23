namespace Game.ConsoleUI.Game
{
    using System;
    using Infrastructure.Helpers;
    using Interfaces;
    using Serilog;

    public class Application : IGameApplication
    {
        private readonly IGameManager gameManager;
        private readonly ILogger logger;

        public Application(ILogger logger, IGameManager gameManager)
        {
            ExceptionHelpers.ThrowOnNullArgument(nameof(gameManager), gameManager);
            ExceptionHelpers.ThrowOnNullArgument(nameof(logger), logger);

            this.logger = logger.ForContext<Application>();
            this.gameManager = gameManager;
        }

        public void Run()
        {
            try
            {
                while (true)
                {
                    this.gameManager.NextTurn();
                }
            }
            catch (Exception e)
            {
                this.logger.Error(e, "Error occured while running application");
            }
        }
    }
}