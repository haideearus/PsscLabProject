using PsscFinalProject.Data.Interface;
using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static PsscFinalProject.Domain.Models.Order;

namespace PsscFinalProject.Domain.Operations
{
    internal sealed class ValidateOrderOperation : OrderOperation
    {
        private readonly IProductRepository productRepository;

        internal ValidateOrderOperation(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        protected override IOrder OnUnvalidated(UnvalidatedOrder unvalidatedOrder)
        {
            (List<ValidatedProduct> validatedProducts, IEnumerable<string> validationErrors) = ValidateListOfProducts(unvalidatedOrder);

            if (validationErrors.Any())
            {
                return new InvalidOrder(unvalidatedOrder.ProductList, validationErrors);
            }
            else
            {
                return new ValidatedOrder(new ClientEmail(unvalidatedOrder.ClientEmail), validatedProducts);
            }
        }

        private (List<ValidatedProduct>, IEnumerable<string>) ValidateListOfProducts(UnvalidatedOrder order)
        {
            List<string> validationErrors = new();
            List<ValidatedProduct> validatedProducts = new();

            foreach (UnvalidatedProduct unvalidatedProduct in order.ProductList)
            {
                ValidatedProduct? validatedProduct = ValidateProduct(unvalidatedProduct, validationErrors);
                if (validatedProduct != null)
                {
                    validatedProducts.Add(validatedProduct);
                }
            }

            return (validatedProducts, validationErrors);
        }

        private ValidatedProduct? ValidateProduct(UnvalidatedProduct unvalidatedProduct, List<string> validationErrors)
        {
            List<string> currentValidationErrors = new();
            ProductCode? productCode = ValidateAndParseProductCode(unvalidatedProduct, currentValidationErrors);
            ProductQuantity? quantity = ValidateAndParseQuantity(unvalidatedProduct, currentValidationErrors);
            ProductName productName = new(unvalidatedProduct.ProductName);
            ProductPrice productPrice = new(unvalidatedProduct.ProductPrice);
            ProductQuantityType quantityType = new(unvalidatedProduct.ProductQuantityType);

            ValidatedProduct? validatedProduct = null;
            if (!currentValidationErrors.Any())
            {
                validatedProduct = new ValidatedProduct(productName, productCode!, productPrice, quantityType, quantity!);
            }
            else
            {
                validationErrors.AddRange(currentValidationErrors);
            }

            return validatedProduct;
        }

        private ProductCode? ValidateAndParseProductCode(UnvalidatedProduct unvalidatedProduct, List<string> validationErrors)
        {
            if (string.IsNullOrEmpty(unvalidatedProduct.ProductCode) || !CheckProductExists(unvalidatedProduct.ProductCode))
            {
                validationErrors.Add($"Invalid product code or product not found ({unvalidatedProduct.ProductCode})");
                return null;
            }

            return new ProductCode(unvalidatedProduct.ProductCode);
        }

        private ProductQuantity? ValidateAndParseQuantity(UnvalidatedProduct unvalidatedProduct, List<string> validationErrors)
        {
            if (unvalidatedProduct.ProductQuantity <= 0)
            {
                validationErrors.Add($"Invalid quantity ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.ProductQuantity})");
                return null;
            }

            return new ProductQuantity(unvalidatedProduct.ProductQuantity);
        }

        private bool CheckProductExists(string code)
        {
            var existingProducts = productRepository.GetExistingProductsAsync(new List<string> { code }).Result;
            return existingProducts.Any(product => product.Value.Equals(code, StringComparison.OrdinalIgnoreCase));
        }
    }
}
