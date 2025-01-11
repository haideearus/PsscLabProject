using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.Order;

namespace PsscFinalProject.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly PsscDbContext dbContext;

        public OrderRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<CalculatedOrder>> GetExistingOrdersAsync()
        {
            // Load orders and clients from the database
            var foundOrders = await(
                from o in dbContext.Orders
                join c in dbContext.Clients on o.ClientId equals c.ClientId
                select new { o.OrderId, o.OrderDate, o.PaymentMethod, o.TotalAmount, o.ShippingAddress, o.State, c.Email, c.ClientId }
            )
            .AsNoTracking()
            .ToListAsync();

            // Map database entities to domain models (CalculatedOrder)
            List<CalculatedOrder> foundOrdersModel = foundOrders.Select(result =>
            {
                // Since we are no longer dealing with products (Bills), we pass an empty list or placeholder.
                var placeholderProductList = new List<CalculatedProduct>(); // Empty list if no products

                return new CalculatedOrder(
                    productList: placeholderProductList,
                    orderId: result.OrderId,
                    orderDate: result.OrderDate,
                    paymentMethod: result.PaymentMethod,
                    totalAmount: result.TotalAmount,
                    shippingAddress: result.ShippingAddress,
                    state: result.State,
                    clientEmail: new ClientEmail(result.Email) // Assuming ClientEmail is a domain model
                );
            }).ToList();

            return foundOrdersModel;
        }

        public async Task SaveOrdersAsync(PaidOrder paidOrder)
        {
            // Load clients by their email (assuming Csv is the client's email)
            ILookup<string, ClientDto> clients = (await dbContext.Clients.ToListAsync())
                .ToLookup(client => client.Email);

            // Add new orders and update existing ones based on the paid order state
            AddNewOrders(paidOrder, clients);
            UpdateExistingOrders(paidOrder, clients);

            // Save changes to the database
            await dbContext.SaveChangesAsync();
        }

        private void UpdateExistingOrders(PaidOrder paidOrder, ILookup<string, ClientDto> clients)
        {
            IEnumerable<OrderDto> updatedOrders = paidOrder.ProductList.Where(o => o.IsUpdated && o.ProductDetailId > 0)
                .Select(o => new OrderDto()
                {
                    OrderId = o.ProductDetailId, // Assuming this is the order ID for the updated order
                    ClientId = clients[paidOrder.Csv].Single().ClientId, // Assuming Csv is related to client identification
                    OrderDate = DateTime.Now, // Assuming current date, adjust if needed
                    PaymentMethod = 1, // Example payment method, adjust if needed
                    TotalAmount = o.TotalPrice, // Assuming the total price comes from the product
                    ShippingAddress = "Unknown", // Adjust according to the model structure
                    State = 1, // Example state, adjust if needed
                });

            foreach (var entity in updatedOrders)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        private void AddNewOrders(PaidOrder paidOrder, ILookup<string, ClientDto> clients)
        {
            IEnumerable<OrderDto> newOrders = paidOrder.ProductList
                .Where(o => !o.IsUpdated && o.ProductDetailId == 0)
                .Select(o => new OrderDto()
                {
                    ClientId = clients[paidOrder.Csv].Single().ClientId, // Assuming Csv is a reference for the client
                    OrderDate = DateTime.Now, // Assuming current date, adjust if necessary
                    PaymentMethod = 1, // Example payment method, adjust if needed
                    TotalAmount = o.TotalPrice, // Assuming the total price comes from the product
                    ShippingAddress = "Unknown", // Adjust according to the model structure
                    State = 1, // Example state, adjust if needed
                });

            dbContext.AddRange(newOrders);
        }
    }
}
