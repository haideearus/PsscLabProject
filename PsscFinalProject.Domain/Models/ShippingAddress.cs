using System;
using System.Text.RegularExpressions;

namespace PsscFinalProject.Domain.Models
{
    public record ShippingAddress
    {
        public const string Pattern = @"(?=.*\bOras\b)(?=.*\bStrada\b)(?=.*\bnr\b)";
        // Address must contain Oras, Strada, and nr

        public string Value { get; }

        public ShippingAddress(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new ArgumentException("Invalid shipping address format. It must include 'Oras,' 'Strada,' and 'nr.'");
            }
        }

        public static ShippingAddress Create(string value)
        {
            return new ShippingAddress(value);
        }

        private static bool IsValid(string stringValue)
        {
            var regex = new Regex(Pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(stringValue);
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool TryParse(string stringValue, out ShippingAddress? shippingAddress)
        {
            bool isValid = false;
            shippingAddress = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                shippingAddress = new(stringValue);
            }

            return isValid;
        }
    }
}
