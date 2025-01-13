using PsscFinalProject.Domain.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace PsscFinalProject.Domain.Models
{
    public record ClientEmail
    {
        public const string Pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        public string Value { get; }

        public ClientEmail(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidClientEmailException("Invalid email format.");
            }
        }

        private static bool IsValid(string stringValue)
        {
            var regex = new Regex(Pattern); 
            return regex.IsMatch(stringValue);
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool TryParse(string stringValue, out ClientEmail? clientEmail)
        {
            bool isValid = false;
            clientEmail = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                clientEmail = new(stringValue);
            }

            return isValid;
        }
    }
}