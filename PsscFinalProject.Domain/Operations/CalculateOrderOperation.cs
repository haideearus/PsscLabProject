using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static PsscFinalProject.Domain.Models.Order;

namespace PsscFinalProject.Domain.Operations
{
    internal class CalculateOrderOperation : OrderOperation
    {
        internal CalculateOrderOperation()
        {
        }

        protected override IOrder OnValidated(ValidatedOrder validatedOrder)
        {
            // Calculate the total amount and price for each product
            List<CalculatedProduct> calculatedProducts = validatedOrder.ProductList
                .Select(validatedProduct => CalculateAndMatchProduct(validatedProduct))
                .ToList();

            decimal totalAmount = calculatedProducts.Sum(product => product.TotalPrice);

            // Handle null ProductId
            var firstProduct = validatedOrder.ProductList.FirstOrDefault();
            if (firstProduct == null || string.IsNullOrEmpty(firstProduct.ProductId))
            {
                throw new InvalidOperationException("Product ID cannot be null or empty.");
            }

            return new CalculatedOrder(
                calculatedProducts.AsReadOnly(),
                orderId: 0,  // Placeholder, you can add actual logic to generate Order ID
                orderDate: DateTime.Now,  // Placeholder, use current time or your business logic for the order date
                paymentMethod: 1, // Placeholder, you may get the actual payment method value
                totalAmount: totalAmount,
                shippingAddress: "", // Placeholder, you may need to extract it from the order input
                state: null, // Placeholder, set an appropriate state based on business logic
                clientEmail: new ClientEmail(firstProduct.ProductId) // Ensure ProductId is not null
            );
        }

        private static CalculatedProduct CalculateAndMatchProduct(ValidatedProduct validatedProduct)
        {
            // Calculate total price: Price * Quantity
            decimal totalPrice = validatedProduct.Price * validatedProduct.Quantity;

            return new CalculatedProduct(
                validatedProduct.ProductId,
                validatedProduct.Name,
                validatedProduct.Price,
                validatedProduct.Quantity,
                totalPrice
            );
        }
    }
}
