using System;
using System.Collections.Generic;

namespace PsscFinalProject.Domain.Models
{
    public record PaymentMethod
    {
        public string Value { get; }

        private static readonly Dictionary<int, string> PaymentMethods = new()
        {
            { 1, "Cash" },
            { 2, "Card" },
            { 3, "PayPal" }
        };

        private PaymentMethod(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !PaymentMethods.ContainsValue(value))
            {
                throw new ArgumentException($"'{value}' is not a valid payment method.");
            }

            Value = value;
        }

        public static PaymentMethod FromString(string value) => new(value);

        public static PaymentMethod FromInt(int value)
        {
            if (!PaymentMethods.TryGetValue(value, out var method))
            {
                throw new ArgumentException($"'{value}' is not a valid payment method ID.");
            }

            return new PaymentMethod(method);
        }

        public int ToInt()
        {
            foreach (var kvp in PaymentMethods)
            {
                if (kvp.Value == Value)
                {
                    return kvp.Key;
                }
            }

            throw new InvalidOperationException("Invalid payment method.");
        }

        public override string ToString() => Value;
    }
}
