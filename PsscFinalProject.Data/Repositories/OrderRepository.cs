using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using PsscFinalProject.Data.Models;
using static PsscFinalProject.Domain.Models.OrderProducts;

namespace PsscFinalProject.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly PsscDbContext dbContext;

        public OrderRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<CalculatedProduct>> GetExistingOrdersAsync()
        {
            // Load orders and join them with products and clients
            var orders = await (
                from o in dbContext.Orders
                join c in dbContext.Clients on o.ClientEmail equals c.Email
                join oi in dbContext.OrderItems on o.OrderId equals oi.OrderItemId
                join p in dbContext.Products on oi.ProductCode equals p.Code
                select new { o, oi, p, c }
            ).ToListAsync();

            // Map results to the domain model
            var calculatedProducts = orders.Select(order => new CalculatedProduct(
                clientEmail: new ClientEmail(order.c.Email),
                productCode: new ProductCode(order.p.Code),
                productPrice: new ProductPrice(order.p.Price),
                productQuantityType: new ProductQuantityType(order.p.QuantityType),
                productQuantity: new ProductQuantity(order.oi.Quantity),
                totalPrice: new ProductPrice(order.oi.Price * order.oi.Quantity)
            )
            {
                OrderItemId = order.oi.OrderItemId,
                OrderId = order.o.OrderId,
                ClientEmail = order.c.Email,
                ProductId = order.p.ProductId
            }).ToList();

            return calculatedProducts;
        }

        public async Task SaveOrdersAsync(PaidOrderProducts paidOrder)
        {
            var order = new OrderDto
            {
                OrderDate = paidOrder.OrderDate,
                PaymentMethod = 1, // Example value
                TotalAmount = paidOrder.TotalAmount.Value,
                ShippingAddress = "Default Address", // Example value
                State = 1, // Example value
                ClientEmail = paidOrder.ClientEmail.Value // Ensure this maps correctly
            };

            dbContext.Orders.Add(order);

            // Save changes to get the generated Order_ID
            await dbContext.SaveChangesAsync();

            // Retrieve the generated Order_ID
            int generatedOrderId = order.OrderId;

            // Add the order items with the generated Order_ID
            foreach (var product in paidOrder.ProductList)
            {
                var orderItem = new OrderItemDto
                {
                    OrderItemId = generatedOrderId, // Use the generated Order_ID
                    ProductCode = product.productCode.Value,
                    Quantity = product.productQuantity.Value,
                    Price = product.productPrice.Value
                };
                dbContext.OrderItems.Add(orderItem);
            }

            // Save the order items
            await dbContext.SaveChangesAsync();
        }
    }
}
