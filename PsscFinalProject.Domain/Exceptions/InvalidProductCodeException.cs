using System;

namespace PsscFinalProject.Domain.Exceptions
{
    [Serializable]
    internal class InvalidProductCodeException : Exception
    {
        public InvalidProductCodeException()
        {
        }

        public InvalidProductCodeException(string? message) : base(message)
        {
        }

        public InvalidProductCodeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}