using System;

namespace PsscFinalProject.Domain.Exceptions
{
    [Serializable]
    internal class InvalidProductPriceException : Exception
    {
        public InvalidProductPriceException()
        {
        }

        public InvalidProductPriceException(string? message) : base(message)
        {
        }

        public InvalidProductPriceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}