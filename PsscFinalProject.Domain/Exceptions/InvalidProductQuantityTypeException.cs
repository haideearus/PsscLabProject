using System;

namespace PsscFinalProject.Domain.Exceptions
{
    [Serializable]
    internal class InvalidProductQuantityTypeException : Exception
    {
        public InvalidProductQuantityTypeException()
        {
        }

        public InvalidProductQuantityTypeException(string? message) : base(message)
        {
        }

        public InvalidProductQuantityTypeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}