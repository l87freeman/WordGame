namespace WordGame.Dictionary.Infrastructure.Helpers
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

        public static void ThrowOnInvalidOperation(string errorMessage)
        {
            throw new InvalidOperationException(errorMessage);
        }
    }
}