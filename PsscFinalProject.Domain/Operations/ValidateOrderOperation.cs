using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.Order;

namespace PsscFinalProject.Domain.Operations
{
        internal sealed class ValidateOrderOperation : OrderOperation
        {
            private readonly Func<string, bool> checkProductExists;

            internal ValidateOrderOperation(Func<string, bool> checkProductExists)
            {
                this.checkProductExists = checkProductExists;
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
                string? productId = ValidateAndParseProductId(unvalidatedProduct, currentValidationErrors);
                int? quantity = ValidateAndParseQuantity(unvalidatedProduct, currentValidationErrors);

                ValidatedProduct? validatedProduct = null;
                if (!currentValidationErrors.Any())
                {
                    validatedProduct = new ValidatedProduct(productId!, unvalidatedProduct.Name, unvalidatedProduct.Price, unvalidatedProduct.Quantity);
                }
                else
                {
                    validationErrors.AddRange(currentValidationErrors);
                }

                return validatedProduct;
            }

            private string? ValidateAndParseProductId(UnvalidatedProduct unvalidatedProduct, List<string> validationErrors)
            {
                if (string.IsNullOrEmpty(unvalidatedProduct.ProductId) || !checkProductExists(unvalidatedProduct.ProductId))
                {
                    validationErrors.Add($"Invalid product ID or product not found ({unvalidatedProduct.ProductId})");
                    return null;
                }

                return unvalidatedProduct.ProductId;
            }

            private int? ValidateAndParseQuantity(UnvalidatedProduct unvalidatedProduct, List<string> validationErrors)
            {
                if (unvalidatedProduct.Quantity <= 0)
                {
                    validationErrors.Add($"Invalid quantity ({unvalidatedProduct.ProductId}, {unvalidatedProduct.Quantity})");
                    return null;
                }

                return unvalidatedProduct.Quantity;
            }
        }
    }

