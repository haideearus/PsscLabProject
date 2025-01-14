using PsscFinalProject.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
        public record ProductName
        {
            public string Value { get; }

            internal ProductName(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidProductNameException("Product name cannot be null or empty.");
                }
                Value = value;
            }

            public override string ToString() => Value;

            public static ProductName Create(string value)
            {
                return new ProductName(value);
            }

        public static bool TryParse(string stringValue, out ProductName? productName)
            {
                bool isValid = false;
                productName = null;

                if (!string.IsNullOrWhiteSpace(stringValue))
                {
                    isValid = true;
                    productName = new ProductName(stringValue);
                }

                return isValid;
            }
        }
}
