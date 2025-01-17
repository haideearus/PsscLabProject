using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsscFinalProject.Domain.Repositories;
using PsscFinalProject.Domain.Models;

namespace PsscFinalProject.Data.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly PsscDbContext dbContext;

        public DeliveryRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await dbContext.Orders
                .Where(o => o.OrderId == orderId)
                .Include(o => o.Client)
                .Include(o => o.Deliveries)
                .FirstOrDefaultAsync();

            return order;
        }

        public async Task<Client> GetClientByEmailAsync(string clientEmail)
        {
            var client = await dbContext.Clients
                .FirstOrDefaultAsync(c => c.Email == clientEmail);

            return client;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            // Step 1: Fetch the OrderDto by orderId
            var orderDto = await dbContext.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (orderDto == null)
            {
                return false; // If no order found, return false
            }

            // Step 2: Create a domain model instance (e.g., Order) from the OrderDto
            var order = new Order.CalculatedOrder(
                orderDto.OrderItems.Select(item => new Order.CalculatedProduct(
                    item.ProductName,
                    item.ProductCode,
                    item.ProductPrice,
                    item.ProductQuantityType,
                    item.ProductQuantity,
                    item.TotalPrice)).ToList(),
                orderDto.OrderDate,
                orderDto.PaymentMethod,
                orderDto.TotalAmount,
                orderDto.ShippingAddress,
                orderDto.State, // Existing state, can be updated to new state
                new ClientEmail(orderDto.ClientEmail) // ClientEmail from the DTO
            );

            // Step 3: Update the State in the domain model
            order.UpdateState((int)status);  // Update the State with the new status

            // Step 4: Update the OrderDto's State in the DB (or update the actual entity directly)
            orderDto.State = (int)status;  // Update the status on the OrderDto

            // Step 5: Save the changes back to the database
            dbContext.Orders.Update(orderDto);  // Update the existing record in the database
            await dbContext.SaveChangesAsync();

            return true;  // Return true indicating the operation succeeded
        }


        public async Task AddPackageDetailsToOrderAsync(int orderId, PackageDetails packageDetails)
        {
            var order = await dbContext.Orders
                .Include(o => o.PackageDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order != null)
            {
                order.PackageDetails = packageDetails;
                dbContext.Orders.Update(order);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}