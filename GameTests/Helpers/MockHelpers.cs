namespace GameTests.Helpers
{
    using Moq;
    using Serilog;

    public static class MockHelpers
    {
        public static Mock<ILogger> LoggerMock<T>()
        {
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.ForContext<T>()).Returns(mockLogger.Object);

            return mockLogger;
        }
    }
}