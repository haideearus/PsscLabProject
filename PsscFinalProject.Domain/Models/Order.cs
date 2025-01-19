using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("PsscFinalProject.Tests")]
[assembly: InternalsVisibleTo("PsscFinalProject.Api")]

namespace PsscFinalProject.Domain.Models
{
    public static class OrderProducts
    {
        public interface IOrderProducts { }

        public record UnvalidatedOrderProducts : IOrderProducts
        {
            public UnvalidatedOrderProducts(IReadOnlyCollection<UnvalidatedOrder> productList)
            {
 
                ProductList = productList ?? throw new ArgumentNullException(nameof(productList));
            }

            public IReadOnlyCollection<UnvalidatedOrder> ProductList { get; }
        }


        public record InvalidOrderProducts : IOrderProducts
        {
            internal InvalidOrderProducts(IReadOnlyCollection<UnvalidatedOrder> productList, IEnumerable<string> reasons)
            {
                ProductList = productList ?? throw new ArgumentNullException(nameof(productList));
                Reasons = reasons ?? throw new ArgumentNullException(nameof(reasons));
            }

            public IReadOnlyCollection<UnvalidatedOrder> ProductList { get; }
            public IEnumerable<string> Reasons { get; }
        }

        public record ValidatedOrderProducts : IOrderProducts          
        {
            internal ValidatedOrderProducts(IReadOnlyCollection<ValidatedOrder> productList)
            {
    
                ProductList = productList ?? throw new ArgumentNullException(nameof(productList));
            }

            public IReadOnlyCollection<ValidatedOrder> ProductList { get; }
        }

        public record CalculatedOrder : IOrderProducts
        {
            internal CalculatedOrder(IReadOnlyCollection<CalculatedProduct> productList, ClientEmail clientEmail)
            {
                ProductList = productList ?? throw new ArgumentNullException(nameof(productList));
                ClientEmail = clientEmail ?? throw new ArgumentNullException( nameof(clientEmail));

            }

            public IReadOnlyCollection<CalculatedProduct> ProductList { get; }
  
            public ClientEmail ClientEmail { get; }
        }

        public record PaidOrderProducts : IOrderProducts
        {
            internal PaidOrderProducts(IReadOnlyCollection<CalculatedProduct> productsList, ProductPrice totalAmount, string csv, DateTime orderDate, ClientEmail clientEmail)
            {
                ProductList = productsList;
                TotalAmount = totalAmount;
                OrderDate = orderDate;
                Csv = csv;
                ClientEmail = clientEmail;

            }
            public ClientEmail ClientEmail { get; }
            public IReadOnlyCollection<CalculatedProduct> ProductList { get; }
            public string Csv { get; }
            public DateTime OrderDate { get; }
            public ProductPrice TotalAmount { get; }

         
        }

      
    }
}