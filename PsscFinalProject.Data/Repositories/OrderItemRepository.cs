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
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly PsscDbContext dbContext;

        public OrderItemRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<CalculatedProduct>> GetProductsAsync()
        {
            // Load order items and join them with products and clients
            var orderItems = await (
                from oi in dbContext.OrderItems
                join o in dbContext.Orders on oi.OrderItemId equals o.OrderId
                join p in dbContext.Products on oi.ProductCode equals p.Code
                join c in dbContext.Clients on o.ClientEmail equals c.Email
                select new { oi, o, p, c }
            ).ToListAsync();

            // Map results to the domain model
            var calculatedProducts = orderItems.Select(item => new CalculatedProduct(
                clientEmail: new ClientEmail(item.c.Email),
                productCode: new ProductCode(item.p.Code),
                productPrice: new ProductPrice(item.p.Price),
                productQuantityType: new ProductQuantityType(item.p.QuantityType),
                productQuantity: new ProductQuantity(item.oi.Quantity),
                totalPrice: new ProductPrice(item.oi.Price * item.oi.Quantity)
            )
            {
                OrderItemId = item.oi.OrderItemId,
                OrderId = item.o.OrderId,
                ClientEmail = item.c.Email,
                ProductId = item.p.ProductId
            }).ToList();

            return calculatedProducts;
        }

        public async Task SaveOrders(PaidOrderProducts order)
        {
            // Fetch existing clients and products from the database
            var clients = await dbContext.Clients.ToDictionaryAsync(client => client.Email);
            var products = await dbContext.Products.ToDictionaryAsync(product => product.Code);

            // Prepare new order items
            var newOrderItems = order.ProductList.Where(p => p.IsUpdated && p.OrderItemId == 0)
                .Select(p => new OrderItemDto
                {
                    OrderItemId = p.OrderId,
                    ProductCode = p.productCode.Value,
                    Price = p.productPrice.Value,
                    Quantity = p.productQuantity.Value
                }).ToList();

            // Prepare updated order items
            var updatedOrderItems = order.ProductList.Where(p => p.IsUpdated && p.OrderItemId > 0)
                .Select(p => new OrderItemDto
                {
                    OrderItemId = p.OrderItemId,
                    ProductCode = p.productCode.Value,
                    Price = p.productPrice.Value,
                    Quantity = p.productQuantity.Value
                }).ToList();

            // Add new order items
            dbContext.OrderItems.AddRange(newOrderItems);

            // Update existing order items
            foreach (var item in updatedOrderItems)
            {
                dbContext.Entry(item).State = EntityState.Modified;
            }

            // Save changes to the database
            await dbContext.SaveChangesAsync();
        }
    }
}
