using System;

namespace PsscFinalProject.Domain.Exceptions
{
    [Serializable]
    internal class InvalidProductQuantityException : Exception
    {
        public InvalidProductQuantityException()
        {
        }

        public InvalidProductQuantityException(string? message) : base(message)
        {
        }

        public InvalidProductQuantityException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}