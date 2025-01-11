using System;

namespace PsscFinalProject.Domain.Exceptions
{
    [Serializable]
    internal class InvalidClientEmailException : Exception
    {
        public InvalidClientEmailException()
        {
        }

        public InvalidClientEmailException(string? message) : base(message)
        {
        }

        public InvalidClientEmailException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}