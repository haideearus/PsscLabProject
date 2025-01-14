using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.Order;

namespace PsscFinalProject.Domain.Models
{
    //public record PublishOrderCommand
    //{
    //    public PublishOrderCommand(IReadOnlyCollection<UnvalidatedOrder> inputOrders)
    //    {
    //       InputOrders= inputOrders;
    //    }
    //    public IReadOnlyCollection<UnvalidatedOrder> InputOrders { get; }
    //    //public string ClientEmail { get; internal set; }
    //}
    //    public record PublishOrderCommand(
    //    string ClientEmail, // Non-nullable
    //    DateTime OrderDate, // Required order date
    //    int PaymentMethod, // Payment method
    //    decimal TotalAmount, // Total amount of the order
    //    string ShippingAddress, // Non-nullable shipping address
    //    int? State // Nullable order state
    //);
    public class PublishOrderCommand
    {
        public string ClientEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } // Required order date
        public int PaymentMethod { get; set; }  // Payment method
        public decimal TotalAmount { get; set; } // Total amount of the order
        public string? ShippingAddress { get; set; } // Nullable shipping address
        public int? State { get; set; } // Nullable order state
        public List<UnvalidatedProduct> ProductList { get; set; } = new(); // Default empty list
    }

}
