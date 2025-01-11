using System;

namespace PsscFinalProject.Domain.Exceptions
{
    [Serializable]
    internal class InvalidOrderStateException : Exception
    {
        public InvalidOrderStateException()
        {
        }

        public InvalidOrderStateException(string? message) : base(message)
        {
        }

        public InvalidOrderStateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}