using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public class OrderItem
    {
        public int OrderId { get; }
        public string ProductCode { get; }
        public decimal Price { get; }
        public int  Quantity { get; }

        public OrderItem(int orderId, string productCode, decimal price, int quantity)
        {
            OrderId = orderId;
            ProductCode = productCode;
            Price = price;
            Quantity = quantity;
        }
    }
}
