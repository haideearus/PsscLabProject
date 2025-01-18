using PsscFinalProject.Domain.Models;
using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.Order;
using PsscFinalProject.Data.Interface;

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
            var foundOrders = await (
                from o in dbContext.Orders
                join c in dbContext.Clients on o.ClientEmail equals c.Email
                select new { o.OrderId, o.OrderDate, o.PaymentMethod, o.TotalAmount, o.ShippingAddress, o.State, c.Email }
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


        public async Task<int> SaveOrdersAsync(PaidOrder paidOrder)
        {
            var order = new OrderDto
            {
                ClientEmail = paidOrder.ClientEmail.Value,
                OrderDate = paidOrder.OrderDate,
                PaymentMethod = paidOrder.PaymentMethod,
                TotalAmount = paidOrder.TotalAmount,
                ShippingAddress = paidOrder.ShippingAddress,
                State = paidOrder.State
            };

            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            return order.OrderId; // Return the generated OrderId
        }


        //public async Task SaveOrdersAsync(PaidOrder paidOrder)
        //{
        //    // Load clients by their email (assuming Csv is the client's email)
        //    ILookup<string, ClientDto> clients = (await dbContext.Clients.ToListAsync())
        //        .ToLookup(client => client.Email);

        //    // Add new orders and update existing ones based on the paid order state
        //    AddNewOrders(paidOrder, clients);
        //    UpdateExistingOrders(paidOrder, clients);

        //    // Save changes to the database
        //    await dbContext.SaveChangesAsync();
        //}

        public async Task SaveOrderWithItemsAsync(CalculatedOrder order, List<CalculatedProduct> products)
        {
            // Insert the main order
            var orderEntity = new OrderDto
            {
                ClientEmail = order.ClientEmail.Value,
                OrderDate = order.OrderDate,
                PaymentMethod = order.PaymentMethod,
                TotalAmount = order.TotalAmount,
                ShippingAddress = order.ShippingAddress,
                State = order.State
            };

            dbContext.Orders.Add(orderEntity);
            await dbContext.SaveChangesAsync();

            // Fetch all products by their codes to validate and map them
            var productCodes = products.Select(p => p.ProductCode.Value).ToList();
            var productEntities = await dbContext.Products
                .Where(p => productCodes.Contains(p.Code))
                .ToListAsync();

            // Insert products into ORDER_ITEM table
            foreach (var product in products)
            {
                var productEntity = productEntities.FirstOrDefault(p => p.Code == product.ProductCode.Value);
                if (productEntity == null)
                {
                    throw new KeyNotFoundException($"Product with code '{product.ProductCode.Value}' not found.");
                }

                var orderItemEntity = new OrderItemDto
                {
                    OrderItemId = orderEntity.OrderId, // Reference the newly created order
                    ProductCode = productEntity.Code, // Use the product's code
                    Price = product.ProductPrice.Value // Use the provided price
                };

                dbContext.OrderItems.Add(orderItemEntity);
            }

            await dbContext.SaveChangesAsync();
        }



        private void UpdateExistingOrders(PaidOrder paidOrder, ILookup<string, ClientDto> clients)
        {
            IEnumerable<OrderDto> updatedOrders = paidOrder.ProductList.Where(o => o.IsUpdated && o.ProductDetailId > 0)
                .Select(o => new OrderDto()
                {
                    ClientEmail = clients[paidOrder.Csv].Single().Email, // Assuming Csv is related to client identification
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

        public async Task<int> AddOrderAsync(PaidOrder paidOrder)
        {
            var order = new OrderDto
            {
                ClientEmail = paidOrder.ClientEmail.Value,
                OrderDate = paidOrder.OrderDate,
                PaymentMethod = paidOrder.PaymentMethod,
                TotalAmount = paidOrder.TotalAmount,
                ShippingAddress = paidOrder.ShippingAddress,
                State = paidOrder.State
            };

            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            return order.OrderId; // Assuming OrderId is auto-generated
        }


        private void AddNewOrders(PaidOrder paidOrder, ILookup<string, ClientDto> clients)
        {
            IEnumerable<OrderDto> newOrders = paidOrder.ProductList
                .Where(o => !o.IsUpdated && o.ProductDetailId == 0)
                .Select(o => new OrderDto()
                {
                    ClientEmail = clients[paidOrder.Csv].Single().Email, // Assuming Csv is a reference for the client
                    OrderDate = DateTime.Now, // Assuming current date, adjust if necessary
                    PaymentMethod = 1, // Example payment method, adjust if needed
                    TotalAmount = o.TotalPrice, // Assuming the total price comes from the product
                    ShippingAddress = "Unknown", // Adjust according to the model structure
                    State = 1, // Example state, adjust if needed
                });

            dbContext.AddRange(newOrders);
        }

        Task IOrderRepository.SaveOrdersAsync(PaidOrder paidOrder)
        {
            throw new NotImplementedException();
        }
    }
}
