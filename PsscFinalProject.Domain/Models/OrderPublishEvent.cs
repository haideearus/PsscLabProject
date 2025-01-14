using System;
using System.Collections.Generic;
using System.Linq;
using static PsscFinalProject.Domain.Models.Order;

namespace PsscFinalProject.Domain.Models
{
    public static class OrderPublishEvent
    {
        public interface IOrderPublishEvent { }

        // Represents the successful publishing of an order

            public record OrderPublishSucceededEvent : IOrderPublishEvent
            {
                public OrderPublishSucceededEvent(string csv, IEnumerable<CalculatedOrderDetail> orderDetails)
                {
                    Csv = csv ?? throw new ArgumentNullException(nameof(csv));
                    OrderDetails = orderDetails?.ToList() ?? throw new ArgumentNullException(nameof(orderDetails));
                }

                public string Csv { get; }
                public List<CalculatedOrderDetail> OrderDetails { get; } // Use concrete type to avoid ambiguity
            }


        // Represents the failure to publish an order
        public record OrderPublishFailedEvent : IOrderPublishEvent
        {
            public IEnumerable<string> Reasons { get; }

            internal OrderPublishFailedEvent(IEnumerable<string> reasons)
            {
                Reasons = reasons;
            }
        }

        // Extension method to convert an IOrder state to a publish event
        public static IOrderPublishEvent ToEvent(this IOrder order) =>
            order switch
            {
                UnvalidatedOrder _ => new OrderPublishFailedEvent(new List<string> { "Unexpected unvalidated state" }),
                ValidatedOrder validatedOrder => new OrderPublishFailedEvent(new List<string> { "Unexpected validated state" }),
                InvalidOrder invalidOrder => new OrderPublishFailedEvent(invalidOrder.Reasons),
                CalculatedOrder calculatedOrder => new OrderPublishSucceededEvent(
                    "CSV Placeholder", // You can replace this with the actual CSV logic
                    ConvertToCalculatedOrderDetails(calculatedOrder.ProductList)
                ),
                PaidOrder paidOrder => new OrderPublishSucceededEvent(
                    paidOrder.Csv,
                    ConvertToCalculatedOrderDetails(paidOrder.ProductList)
                ),
                _ => throw new NotImplementedException("Unrecognized order state")
            };

        // Helper method to convert `CalculatedProduct` into `CalculatedOrderDetail`
        private static IEnumerable<CalculatedOrderDetail> ConvertToCalculatedOrderDetails(IEnumerable<CalculatedProduct> products)
        {
            return products.Select(product => new CalculatedOrderDetail(
                product.ProductCode.Value,
                product.ProductPrice.Value,
                product.ProductQuantity.Value,
                product.TotalPrice
            ));
        }
    }

    // Domain model for calculated order details (to be used in the event)
    public record CalculatedOrderDetail
    {
        public string ProductId { get; }
        public decimal Price { get; }
        public int Quantity { get; }
        public decimal TotalPrice { get; }

        public CalculatedOrderDetail(string productId, decimal price, int quantity, decimal totalPrice)
        {
            ProductId = productId;
            Price = price;
            Quantity = quantity;
            TotalPrice = totalPrice;
        }
    }
}
