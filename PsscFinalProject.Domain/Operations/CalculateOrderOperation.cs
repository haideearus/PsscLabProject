using Microsoft.Extensions.Logging;
using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static PsscFinalProject.Domain.Models.Order;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PsscFinalProject.Tests")]


namespace PsscFinalProject.Domain.Operations
{
    internal class CalculateOrderOperation : OrderOperation
    {
        internal CalculateOrderOperation() { }

        public override IOrder Transform(IOrder? order, object? state)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null");
            }

            return order switch
            {
                ValidatedOrder validatedOrder => OnValidated(validatedOrder),
                _ => throw new InvalidOperationException("Unexpected order type")
            };
        }


        protected override IOrder OnValidated(ValidatedOrder validatedOrder)
        {
            if (validatedOrder == null)
            {
                throw new ArgumentNullException(nameof(validatedOrder), "Validated order cannot be null");
            }

            // Calculate total price for each product
            var calculatedProducts = validatedOrder.ProductList
                .Select(product =>
                {
                    var totalPrice = product.ProductPrice.Value * product.ProductQuantity.Value;
                    return new CalculatedProduct(
                        product.ProductName,
                        product.ProductCode,
                        product.ProductPrice,
                        product.ProductQuantityType,
                        product.ProductQuantity,
                        totalPrice
                    );
                }).ToList();

            // Calculate total order amount
            var totalAmount = calculatedProducts.Sum(product => product.TotalPrice);

            // Define default values for properties
            const string defaultShippingAddress = "Default Address";
            const int defaultPaymentMethod = 1; // Example: 1 for "Cash on Delivery"
            const int orderState = 1; // Example: 1 for "Placed"

            // Create and return a CalculatedOrder
            return new CalculatedOrder(
                productList: calculatedProducts.AsReadOnly(),
                orderDate: DateTime.Now,
                paymentMethod: defaultPaymentMethod,
                totalAmount: totalAmount,
                shippingAddress: defaultShippingAddress,
                state: orderState,
                clientEmail: validatedOrder.Email
            );
        }


        private static CalculatedProduct CalculateProduct(ValidatedProduct validatedProduct)
        {
            if (validatedProduct.ProductPrice.Value <= 0 || validatedProduct.ProductQuantity.Value <= 0)
            {
                throw new InvalidOperationException($"Invalid product price or quantity for {validatedProduct.ProductCode.Value}");
            }

            var totalPrice = validatedProduct.ProductPrice.Value * validatedProduct.ProductQuantity.Value;

            return new CalculatedProduct(
                validatedProduct.ProductName,
                validatedProduct.ProductCode,
                validatedProduct.ProductPrice,
                validatedProduct.ProductQuantityType,
                validatedProduct.ProductQuantity,
                totalPrice
            );
        }

    }
}
