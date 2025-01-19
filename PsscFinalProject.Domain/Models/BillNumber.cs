using System;
using System.Text.RegularExpressions;

namespace PsscFinalProject.Domain.Models
{
    public record BillNumber
    {
        public const string Pattern = @"^AWS\d{5}$"; // AWS followed by 5 digits

        public string Value { get; }

        public BillNumber(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new ArgumentException("Invalid bill number format. It must match 'AWS' followed by 5 digits.");
            }
        }

        public static BillNumber Create(string value)
        {
            return new BillNumber(value);
        }

        public static BillNumber Generate()
        {
            var random = new Random();
            var randomDigits = random.Next(10000, 100000); // Generate a 5-digit number
            return new BillNumber($"AWS{randomDigits}");
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

        public static bool TryParse(string stringValue, out BillNumber? billNumber)
        {
            bool isValid = false;
            billNumber = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                billNumber = new(stringValue);
            }

            return isValid;
        }
    }
}
