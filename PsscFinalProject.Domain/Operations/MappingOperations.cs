using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Operations
{
    public static class MappingOperations
    {
        public static Bill MapBillDtoToBill(BillDto billDto, OrderDto orderDto)
        {
            Client client = new Client(orderDto.Client.ClientId, orderDto.Client.Email);

            PaymentMethod paymentMethod = (PaymentMethod)orderDto.PaymentMethod;

            return new Bill
            {
                BillId = billDto.BillId,
                OrderId = billDto.OrderId,
                TotalAmount = billDto.TotalAmount,
                BillingDate = billDto.BillingDate,
                Order = new Order(client, orderDto.OrderDate, paymentMethod, orderDto.TotalAmount, orderDto.ShippingAddress)
            };
        }
        public static Product MapProductDtoToProduct(ProductDto productDto)
        {
            return new Product
            {
                ProductId = productDto.ProductId,
                Name = productDto.Name,
                Code = productDto.Code,
                Price = productDto.Price,
                QuantityType = productDto.QuantityType,
                Stock = productDto.Stock
            };
        }
    }
}