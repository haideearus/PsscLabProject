using System;

namespace PsscFinalProject.Domain.Exceptions
{
    [Serializable]
    internal class InvalidProductNameException : Exception
    {
        public InvalidProductNameException()
        {
        }

        public InvalidProductNameException(string? message) : base(message)
        {
        }

        public InvalidProductNameException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}