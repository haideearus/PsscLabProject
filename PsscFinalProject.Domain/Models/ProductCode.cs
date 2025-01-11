using PsscFinalProject.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace PsscFinalProject.Domain.Models
{
    public record ProductCode
    {
        private static readonly Regex ValidPattern = new(@"^PRD[0-9]{3}$");

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

        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

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
