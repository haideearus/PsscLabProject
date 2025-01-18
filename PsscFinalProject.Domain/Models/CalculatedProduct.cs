using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public record CalculatedProduct(
           ClientEmail clientEmail,
            ProductCode productCode,
            ProductPrice productPrice,
            ProductQuantityType productQuantityType,
            ProductQuantity productQuantity,
            ProductPrice totalPrice)
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public string? ClientEmail {  get; set; }
        public int ProductId { get; set; }
        public bool IsUpdated { get; init; }
        public bool IsUpdatedLine { get; set; }
    }
}
