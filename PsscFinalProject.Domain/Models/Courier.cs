using System;

namespace PsscFinalProject.Domain.Models
{
    public record Courier
    {
        public string Value { get; }

        private static readonly string[] ValidCouriers = { "Sameday", "GLS", "FanCourier", "Posta" }; 

        public Courier(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Courier type cannot be null or empty.");
            }

            if (Array.IndexOf(ValidCouriers, value) == -1)
            {
                throw new ArgumentException($"'{value}' is not a valid courier type.");
            }

            Value = value;
        }

        public static Courier Create(string value)
        {
            return new Courier(value);
        }

        public override string ToString() => Value;

        public static bool TryParse(string? value, out Courier? courier)
        {
            courier = null;

            if (!string.IsNullOrWhiteSpace(value) && Array.IndexOf(ValidCouriers, value) != -1)
            {
                courier = new Courier(value);
                return true;
            }

            return false;
        }
    }
}
