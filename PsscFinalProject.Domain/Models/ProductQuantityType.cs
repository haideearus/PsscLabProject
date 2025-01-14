using PsscFinalProject.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public record ProductQuantityType
    {
            public string Value { get; }

            private static readonly string[] ValidTypes = { "Piece", "Kilogram", "Liter", "Unit" }; // Example valid types

            internal ProductQuantityType(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidProductQuantityTypeException("Quantity type cannot be null or empty.");
                }

                if (Array.IndexOf(ValidTypes, value) == -1)
                {
                    throw new InvalidProductQuantityTypeException($"'{value}' is not a valid quantity type.");
                }

                Value = value;
            }

        public static ProductQuantityType Create(string value)
        {
            return new ProductQuantityType(value);
        }
        public override string ToString() => Value;

            public static bool TryParse(string? value, out ProductQuantityType? quantityType)
            {
                quantityType = null;

                if (!string.IsNullOrWhiteSpace(value) && Array.IndexOf(ValidTypes, value) != -1)
                {
                    quantityType = new ProductQuantityType(value);
                    return true;
                }

                return false;
            }
        }
 }
