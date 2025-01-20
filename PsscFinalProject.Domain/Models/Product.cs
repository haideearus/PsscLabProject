using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public record UnvalidatedProduct
    {
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public decimal? ProductPrice { get; set; }
        public string? ProductQuantityType { get; set; }
        public int? ProductQuantity { get; set; }

        public UnvalidatedProduct(string productName, string productCode, decimal productPrice, string productQuantityType, int productQuantity)
        {
            ProductName = productName;
            ProductCode = productCode;
            ProductPrice = productPrice;
            ProductQuantityType = productQuantityType;
            ProductQuantity = productQuantity;
        }
    }

    public record ValidatedProduct(
         ProductName ProductName,
         ProductCode ProductCode,
         ProductPrice ProductPrice,
         ProductQuantityType ProductQuantityType,
         ProductQuantity ProductQuantity);


}
