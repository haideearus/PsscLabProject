using System;
using System.Collections.Generic;
using System.Linq;

namespace PsscFinalProject.Domain.Models
{
        public static class Order
        {
            public interface IOrder { }

            // Represents the unvalidated state of the order
            public record UnvalidatedOrder : IOrder
            {
                public UnvalidatedOrder(IReadOnlyCollection<UnvalidatedProduct> productList)
                {
                    ProductList = productList;
                }

                public IReadOnlyCollection<UnvalidatedProduct> ProductList { get; }
            }

            // Represents the invalid state of the order (if validation failed)
            public record InvalidOrder : IOrder
            {
                internal InvalidOrder(IReadOnlyCollection<UnvalidatedProduct> productList, IEnumerable<string> reasons)
                {
                    ProductList = productList;
                    Reasons = reasons;
                }

                public IReadOnlyCollection<UnvalidatedProduct> ProductList { get; }
                public IEnumerable<string> Reasons { get; }
            }

            // Represents the validated state of the order (after successful validation)
            public record ValidatedOrder : IOrder
            {
                internal ValidatedOrder(IReadOnlyCollection<ValidatedProduct> productList)
                {
                    ProductList = productList;
                }

                public IReadOnlyCollection<ValidatedProduct> ProductList { get; }
            }

            // Represents the calculated state of the order (with totals calculated for each product)
            public record CalculatedOrder : IOrder
            {
                internal CalculatedOrder(IReadOnlyCollection<CalculatedProduct> productList)
                {
                    ProductList = productList;
                }

                public IReadOnlyCollection<CalculatedProduct> ProductList { get; }
            }

        // Represents the paid state of the order (after payment has been processed)
        public record PaidOrder : IOrder
        {
            internal PaidOrder(IReadOnlyCollection<CalculatedProduct> productList, string csv, DateTime paidDate)
            {
                ProductList = productList;
                PaidDate = paidDate;
                Csv = csv;
            }

            public IReadOnlyCollection<CalculatedProduct> ProductList { get; }
            public DateTime PaidDate { get; }
            public string Csv { get; }
        }

        // The base structures for product details used in the order

        // Represents a product in the unvalidated order (before any validation)
        public record UnvalidatedProduct(string ProductId, string Name, decimal Price, int Quantity);

            // Represents a product in the validated order (after passing validation)
            public record ValidatedProduct(string ProductId, string Name, decimal Price, int Quantity);

            // Represents a product in the calculated order (after calculations like price totals are done)
            public record CalculatedProduct(string ProductId, string Name, decimal Price, int Quantity, decimal TotalPrice)
            {
                public int ProductDetailId { get; set; }
                public bool IsUpdated { get; set; }
            }
        }
    }