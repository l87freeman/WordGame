namespace GameTests
{
    using System;
    using Game.ConsoleUI.Game;
    using Game.ConsoleUI.Interfaces;
    using GameTests.Helpers;
    using Moq;
    using Serilog;
    using Xunit;

    public class GameApplicationTests
    {
        [Fact]
        public void Ctor_ThrowOnInvalidArguments()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var app = new Application(null, Mock.Of<IGameManager>());
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var app = new Application(Mock.Of<ILogger>(), null);
            });
        }

        [Fact]
        public void Run_ShouldRunUntilException()
        {
            var expectedInvocations = 6;
            var invocations = 0;

            var loggerMock = MockHelpers.LoggerMock<Application>();

            var gameManagerMock = new Mock<IGameManager>();
            gameManagerMock.Setup(m => m.NextTurn()).Callback(() => {
                if (++invocations == expectedInvocations)
                {
                    throw new Exception();
                }
            });

            var app = this.CreateApplication(loggerMock.Object, gameManagerMock.Object);
            app.Run();

            gameManagerMock.Verify(m => m.NextTurn(), Times.Exactly(expectedInvocations));
            loggerMock.Verify(m => m.Error(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }

        private Application CreateApplication(ILogger logger, IGameManager manager)
        {
            var app = new Application(logger ?? Mock.Of<ILogger>(), manager ?? Mock.Of<IGameManager>());
            return app;
        }
    }
}