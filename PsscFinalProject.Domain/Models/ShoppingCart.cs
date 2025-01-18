using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.OrderProducts;

namespace PsscFinalProject.Domain.Models
{
    public class ShoppingCart
    {
        private readonly List<ValidatedProduct> _products = new();

        public void AddProduct(ValidatedProduct product)
        {
            _products.Add(product);
        }

        public void RemoveProduct(string productCode)
        {
            _products.RemoveAll(p => p.ProductCode.Value == productCode);
        }

        public IReadOnlyCollection<ValidatedProduct> GetProducts()
        {
            return _products.AsReadOnly();
        }

        public decimal CalculateTotalPrice()
        {
            return _products.Sum(p => p.ProductPrice.Value);
        }

        public bool CheckStock()
        {
            return _products.All(p => p.ProductQuantity.Value > 0);
        }
    }
}
