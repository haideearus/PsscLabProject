using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using static PsscFinalProject.Domain.Models.OrderProducts;

namespace PsscFinalProject.Domain.Operations
{
    internal sealed class ValidateOrderOperation : OrderOperation
    {
        private readonly IProductRepository productRepository;

        internal ValidateOrderOperation(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        protected override IOrderProducts OnUnvalidated(UnvalidatedOrderProducts unvalidatedOrder)
        {
            var (validatedProducts, validationErrors) = ValidateProducts(unvalidatedOrder);

            if (validationErrors.Any())
            {
                return new InvalidOrderProducts(unvalidatedOrder.ProductList, validationErrors);
            }

            return new ValidatedOrderProducts(validatedProducts);
        }

        private (List<ValidatedOrder>, IEnumerable<string>) ValidateProducts(UnvalidatedOrderProducts unvalidatedOrder)
        {
            var validationErrors = new List<string>();
            var validatedProducts = new List<ValidatedOrder>();

            foreach (var product in unvalidatedOrder.ProductList)
            {
                var validationResult = ValidateProduct(product);

                if (validationResult.IsValid && validationResult.ValidatedProduct != null)
                {
                    validatedProducts.Add(validationResult.ValidatedProduct);
                }
                else
                {
                    validationErrors.AddRange(validationResult.Errors);
                }
            }

            return (validatedProducts, validationErrors);
        }

        private (bool IsValid, ValidatedOrder? ValidatedProduct, List<string> Errors) ValidateProduct(UnvalidatedOrder product)
        {
            var errors = new List<string>();

            var productCode = ValidateProductCode(product.ProductCode, errors);
            var quantity = ValidateQuantity(product.Quantity, errors);

            if (!errors.Any() && productCode != null && quantity != null)
            {
                return (true, new ValidatedOrder(new ClientEmail(product.ClientEmail), productCode, quantity), errors);
            }

            return (false, null, errors);
        }

        private ProductCode? ValidateProductCode(string productCode, List<string> errors)
        {
            var existingProducts = productRepository.GetExistingProductsAsync(new List<string> { productCode }).Result;

            if (!existingProducts.Any(p => p.Value == productCode))
            {
                errors.Add($"Invalid product code: {productCode}");
                return null;
            }

            return new ProductCode(productCode);
        }

        private ProductQuantity? ValidateQuantity(int quantity, List<string> errors)
        {
            if (quantity <= 0)
            {
                errors.Add($"Invalid quantity: {quantity}");
                return null;
            }

            return new ProductQuantity(quantity);
        }
    }
}
