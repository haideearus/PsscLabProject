﻿using PsscFinalProject.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace PsscFinalProject.Domain.Models
{
    public record ProductCode
    {
        public const string Pattern = @"^PRD\d{3}$";

        public string Value { get; }

        public ProductCode(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductCodeException("Invalid product code format.");
            }
        }

        public static ProductCode Create(string value)
        {
            return new ProductCode(value);
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

        public static bool TryParse(string stringValue, out ProductCode? productCode)
        {
            bool isValid = false;
            productCode = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                productCode = new(stringValue);
            }

            return isValid;
        }
    }
}