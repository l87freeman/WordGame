namespace Game.ConsoleUI.Infrastructure.Helpers
{
    using System;

    public class ExceptionHelpers
    {
        public static void ThrowOnNullArgument(string argumentName, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException($"{argumentName} should not be null");
            }
        }

        public static void ThrowOnInvalidOperationArgument(string argumentName, Func<bool> isValidFunc)
        {
            ThrowOnInvalidOperation($"{argumentName} is not valid", isValidFunc);
        }

        public static void ThrowOnInvalidOperation(string message, Func<bool> isValidFunc)
        {
            if (!isValidFunc())
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}