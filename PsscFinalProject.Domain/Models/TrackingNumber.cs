using System;
using System.Text.RegularExpressions;

namespace PsscFinalProject.Domain.Models
{
    public record TrackingNumber
    {
        public const string Pattern = @"^TRK\d{5}$"; // TRK followed by 5 digits

        public string Value { get; }

        public TrackingNumber(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new ArgumentException("Invalid tracking number format. It must match 'TRK' followed by 5 digits.");
            }
        }

        public static TrackingNumber Create(string value)
        {
            return new TrackingNumber(value);
        }

        public static TrackingNumber Generate()
        {
            var random = new Random();
            var randomDigits = random.Next(10000, 100000); // Generate a 5-digit number
            return new TrackingNumber($"TRK{randomDigits}");
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

        public static bool TryParse(string stringValue, out TrackingNumber? trackingNumber)
        {
            bool isValid = false;
            trackingNumber = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                trackingNumber = new(stringValue);
            }

            return isValid;
        }
    }
}
