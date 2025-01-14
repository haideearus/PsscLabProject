using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
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
                return new ValidatedOrder(validatedProducts);
            }
        }

        private (List<ValidatedProduct>, IEnumerable<string>) ValidateListOfProducts(UnvalidatedOrder order)
        {
            List<string> validationErrors = new List<string>();
            List<ValidatedProduct> validatedProducts = new List<ValidatedProduct>();

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
            List<string> currentValidationErrors = new List<string>();
            string? productCode = ValidateAndParseProductCode(unvalidatedProduct, currentValidationErrors);
            int? quantity = ValidateAndParseQuantity(unvalidatedProduct, currentValidationErrors);

            ValidatedProduct? validatedProduct = null;
            if (!currentValidationErrors.Any())
            {
                validatedProduct = new ValidatedProduct(productCode!, unvalidatedProduct.Name, unvalidatedProduct.Price, unvalidatedProduct.Quantity);
            }
            else
            {
                validationErrors.AddRange(currentValidationErrors);
            }

            return validatedProduct;
        }

        private string? ValidateAndParseProductCode(UnvalidatedProduct unvalidatedProduct, List<string> validationErrors)
        {
            if (string.IsNullOrEmpty(unvalidatedProduct.Code) || !checkProductExists(unvalidatedProduct.Code))
            {
                validationErrors.Add($"Invalid product code or product not found ({unvalidatedProduct.Code})");
                return null;
            }

            return unvalidatedProduct.Code;
        }

        private int? ValidateAndParseQuantity(UnvalidatedProduct unvalidatedProduct, List<string> validationErrors)
        {
            if (unvalidatedProduct.Quantity <= 0)
            {
                validationErrors.Add($"Invalid quantity ({unvalidatedProduct.Code}, {unvalidatedProduct.Quantity})");
                return null;
            }

            return unvalidatedProduct.Quantity;
        }

        private bool checkProductExists(string code)
        {
            var existingProducts = productRepository.GetExistingProductsAsync(new List<string> { code }).Result;
            return existingProducts.Any(product => product.Value.Equals(code, StringComparison.OrdinalIgnoreCase));
        }
    }
}