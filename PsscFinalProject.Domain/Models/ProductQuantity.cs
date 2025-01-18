using PsscFinalProject.Domain.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public record ProductQuantity
    {
        public int Value { get; }

        public ProductQuantity(int value)
        {
            if (value < 0)
            {
                throw new InvalidProductQuantityException("Stock cannot be negative.");
            }
            Value = value;
        }

        public override string ToString() => Value.ToString();

        public static ProductQuantity Create(int value)
        {
            return new ProductQuantity(value);
        }

        public static bool TryParse(int? value, out ProductQuantity? stock)
        {
            stock = null;

            if (value.HasValue && value.Value >= 0)
            {
                stock = new ProductQuantity(value.Value);
                return true;
            }

            return false;
        }
    }
}
