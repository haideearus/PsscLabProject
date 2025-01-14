using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsscFinalProject.Data.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly PsscDbContext dbContext;

        public OrderItemRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<ValidatedProduct>> GetOrderItemsAsync()
        {
            var orderItems = await dbContext.OrderItems.AsNoTracking().ToListAsync();
            return orderItems.Select(dto => MapToDomain(dto)).ToList();
        }

        public async Task SaveOrderItemsAsync(IEnumerable<ValidatedProduct> orderItems)
        {
            var dtos = orderItems.Select(domain => MapToDto(domain));
            dbContext.OrderItems.AddRange(dtos.Where(oi => oi.OrderItemId == 0));
            dbContext.OrderItems.UpdateRange(dtos.Where(oi => oi.OrderItemId > 0));
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteOrderItemAsync(int orderItemId)
        {
            var orderItem = await dbContext.OrderItems.FindAsync(orderItemId);
            if (orderItem != null)
            {
                dbContext.OrderItems.Remove(orderItem);
                await dbContext.SaveChangesAsync();
            }
        }

        //public async Task AddOrderItemAsync(int orderId, string productCode, decimal price)
        //{
        //    var orderItem = new OrderItemDto
        //    {
        //        OrderItemId = orderId,
        //        ProductCode = productCode,
        //        Price = price
        //    };

        //    dbContext.OrderItems.Add(orderItem);
        //    await dbContext.SaveChangesAsync();
        //}

        public async Task AddOrderItemAsync(int orderId, string productCode, decimal price)
        {
            var orderItem = new OrderItemDto
            {
                OrderItemId = orderId,
                ProductCode = productCode,
                Price = price
            };

            dbContext.OrderItems.Add(orderItem);
            await dbContext.SaveChangesAsync();
        }


        public async Task AddOrderItemsAsync(IEnumerable<ValidatedProduct> orderItems)
        {
            var dtos = orderItems.Select(domain => MapToDto(domain));
            dbContext.OrderItems.AddRange(dtos);
            await dbContext.SaveChangesAsync();
        }

        private static ValidatedProduct MapToDomain(OrderItemDto dto)
        {
            return new ValidatedProduct(
                ProductName.Create(" "), // Replace with actual logic
                ProductCode.Create(dto.ProductCode),
                ProductPrice.Create(dto.Price),
                ProductQuantityType.Create("Quantity Type Placeholder"), // Replace with actual logic
                ProductQuantity.Create(1) // Replace with actual logic
            );
        }

        private static OrderItemDto MapToDto(ValidatedProduct domain)
        {
            return new OrderItemDto
            {
                OrderItemId = 0, // Assume new item; update as needed
                ProductCode = domain.ProductCode.Value,
                Price = domain.ProductPrice.Value
            };
        }
    }
}
