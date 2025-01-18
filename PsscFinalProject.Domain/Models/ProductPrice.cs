using PsscFinalProject.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public record ProductPrice
    {
        public decimal Value { get; }

        public ProductPrice(decimal value)
        {
            if (value <= 0)
            {
                throw new InvalidProductPriceException("Price must be greater than zero.");
            }
            Value = value;
        }

        public override string ToString() => $"{Value:C}";

        public static ProductPrice Create(decimal value)
        {
            return new ProductPrice(value);
        }
        public static bool TryParse(decimal? value, out ProductPrice? price)
        {
            price = null;

            if (value.HasValue && value.Value > 0)
            {
                price = new ProductPrice(value.Value);
                return true;
            }

            return false;
        }
    }
}