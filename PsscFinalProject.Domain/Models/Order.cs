using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("PsscFinalProject.Tests")]


namespace PsscFinalProject.Domain.Models
{
        public static class Order
        {
            public interface IOrder { }

        public record UnvalidatedOrder : IOrder
        {
            public UnvalidatedOrder(string clientEmail, IReadOnlyCollection<UnvalidatedProduct> productList)
            {
                ClientEmail = clientEmail ?? throw new ArgumentNullException(nameof(clientEmail));
                ProductList = productList ?? throw new ArgumentNullException(nameof(productList));
            }

            public IReadOnlyCollection<UnvalidatedProduct> ProductList { get; }
            public string ClientEmail { get; set; }
        }


        public record InvalidOrder : IOrder
        {
            internal InvalidOrder(IReadOnlyCollection<UnvalidatedProduct> productList, IEnumerable<string> reasons)
            {
                ProductList = productList ?? throw new ArgumentNullException(nameof(productList));
                Reasons = reasons ?? throw new ArgumentNullException(nameof(reasons));
            }

            public IReadOnlyCollection<UnvalidatedProduct> ProductList { get; }
            public IEnumerable<string> Reasons { get; }
        }

        public record ValidatedOrder : IOrder
        {
            internal ValidatedOrder(ClientEmail clientEmail, IReadOnlyCollection<ValidatedProduct> productList)
            {
                Email = clientEmail ?? throw new ArgumentNullException(nameof(clientEmail));
                ProductList = productList ?? throw new ArgumentNullException(nameof(productList));
            }

            public IReadOnlyCollection<ValidatedProduct> ProductList { get; }
            public ClientEmail Email { get; set; }
        }

        public record CalculatedOrder : IOrder
        {
            public CalculatedOrder(
                IReadOnlyCollection<CalculatedProduct> productList,
                DateTime orderDate,
                int paymentMethod,
                decimal totalAmount,
                string shippingAddress,
                int? state,
                ClientEmail clientEmail)
            {
                ProductList = productList ?? throw new ArgumentNullException(nameof(productList));
                OrderDate = orderDate;
                PaymentMethod = paymentMethod;
                TotalAmount = totalAmount;
                ShippingAddress = shippingAddress ?? throw new ArgumentNullException(nameof(shippingAddress));
                State = state;
                ClientEmail = clientEmail ?? throw new ArgumentNullException(nameof(clientEmail));
            }

            public IReadOnlyCollection<CalculatedProduct> ProductList { get; }
            public DateTime OrderDate { get; }
            public int PaymentMethod { get; }
            public decimal TotalAmount { get; }
            public string ShippingAddress { get; }
            public int? State { get; }
            public ClientEmail ClientEmail { get; }
        }

        public record PaidOrder : IOrder
        {
            public PaidOrder(
                ClientEmail clientEmail,
                IReadOnlyCollection<CalculatedProduct> productList,
                string csv,
                DateTime orderDate,
                int paymentMethod,
                decimal totalAmount,
                string shippingAddress,
                int? state = null,
                int? orderId = null)
            {
                ClientEmail = clientEmail ?? throw new ArgumentNullException(nameof(clientEmail));
                ProductList = productList ?? throw new ArgumentNullException(nameof(productList));
                Csv = csv ?? throw new ArgumentNullException(nameof(csv));
                OrderDate = orderDate;
                PaymentMethod = paymentMethod;
                TotalAmount = totalAmount;
                ShippingAddress = shippingAddress ?? throw new ArgumentNullException(nameof(shippingAddress));
                State = state;
                OrderId = orderId;
            }

            public ClientEmail ClientEmail { get; }
            public IReadOnlyCollection<CalculatedProduct> ProductList { get; }
            public string Csv { get; }
            public DateTime OrderDate { get; }
            public int PaymentMethod { get; }
            public decimal TotalAmount { get; }
            public string ShippingAddress { get; }
            public int? State { get; }
            public int? OrderId { get; } // Nullable OrderId for tracking
        }

        public record CalculatedProduct(ProductName ProductName, ProductCode ProductCode, ProductPrice ProductPrice, ProductQuantityType ProductQuantityType ,ProductQuantity ProductQuantity, decimal TotalPrice)
            {
                public int ProductDetailId { get; set; }
                public bool IsUpdated { get; set; }
            }
        }
}