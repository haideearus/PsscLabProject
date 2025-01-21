using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using PsscFinalProject.Data.Models;
using static PsscFinalProject.Domain.Models.OrderProducts;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Logging;
using PsscFinalProject.Domain.Operations;

namespace PsscFinalProject.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly PsscDbContext dbContext;
        private readonly OrderStateOperation orderStateOperation;

        public OrderRepository(PsscDbContext dbContext, OrderStateOperation orderStateOperation)
        {
            this.dbContext = dbContext;
            this.orderStateOperation = orderStateOperation; 
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

            foreach (var order in orders)
            {
                var newState = orderStateOperation.CalculateOrderState(order.o.OrderDate);
                await UpdateOrderStateAsync(order.o.OrderId, newState); 
            }

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

        public async Task<PaidOrderProducts> GetOrderByIdAsync(int orderId)
        {
            var order = await (
                from o in dbContext.Orders
                join oi in dbContext.OrderItems on o.OrderId equals oi.OrderItemId
                join p in dbContext.Products on oi.ProductCode equals p.Code
                where o.OrderId == orderId
                select new { o, oi, p }
            ).ToListAsync();

            if (!order.Any())
            {
                return null; 
            }

            var newState = orderStateOperation.CalculateOrderState(order.First().o.OrderDate);
            await UpdateOrderStateAsync(order.First().o.OrderId, newState);

            var paidOrder = new PaidOrderProducts(
                productsList: order.Select(o => new CalculatedProduct(
                    clientEmail: new ClientEmail(o.o.ClientEmail),
                    productCode: new ProductCode(o.p.Code),
                    productPrice: new ProductPrice(o.p.Price),
                    productQuantityType: new ProductQuantityType(o.p.QuantityType),
                    productQuantity: new ProductQuantity(o.oi.Quantity),
                    totalPrice: new ProductPrice(o.oi.Price * o.oi.Quantity)
                )).ToList(),
                totalAmount: new ProductPrice(order.Sum(o => o.oi.Price * o.oi.Quantity)),
                csv: string.Empty,
                orderDate: order.First().o.OrderDate,
                clientEmail: new ClientEmail(order.First().o.ClientEmail)
            );

            return paidOrder;
        }

        public async Task SaveOrdersAsync(PaidOrderProducts paidOrder)
        {
            // Fetch the shipping address and payment method for the client
            var addressInfo = await dbContext.Addresses
                .Where(a => a.ClientEmail == paidOrder.ClientEmail.Value)
                .Select(a => new
                {
                    ShippingAddress = a.ClientAddress,
                    PaymentMethod = a.PaymentMethod // Retrieve as integer
                })
                .FirstOrDefaultAsync();

            if (addressInfo != null)
            {
                // Create and add the order
                var order = new OrderDto
                {
                    OrderDate = paidOrder.OrderDate,
                    PaymentMethod = addressInfo.PaymentMethod, // Store as integer
                    TotalAmount = paidOrder.TotalAmount.Value,
                    ShippingAddress = addressInfo.ShippingAddress,
                    State = (int)OrderState.Pending,
                    ClientEmail = paidOrder.ClientEmail.Value
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
                        OrderItemId = generatedOrderId, // Associate with the generated order
                        ProductCode = product.productCode.Value,
                        Quantity = product.productQuantity.Value,
                        Price = product.productPrice.Value
                    };

                    dbContext.OrderItems.Add(orderItem);
                }

                // Save all order items in one operation
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("No address or payment method found for the specified client.");
            }
        }


        public async Task<List<CalculatedProduct>> GetOrdersByEmailAsync(string email)
        {
            var orders = await(
                from o in dbContext.Orders
                join c in dbContext.Clients on o.ClientEmail equals c.Email
                join oi in dbContext.OrderItems on o.OrderId equals oi.OrderItemId
                join p in dbContext.Products on oi.ProductCode equals p.Code
                where o.ClientEmail == email
                select new
                {
                    Order = o,
                    OrderItem = oi,
                    Product = p,
                    Client = c,
                    OrderItemId = oi.OrderItemId
                }
            ).ToListAsync();

            foreach (var order in orders)
            {
                var newState = orderStateOperation.CalculateOrderState(order.Order.OrderDate);
                await UpdateOrderStateAsync(order.Order.OrderId, newState); 
            }

            return orders.Select(order => new CalculatedProduct(
                clientEmail: new ClientEmail(order.Client.Email),
                productCode: new ProductCode(order.Product.Code),
                productPrice: new ProductPrice(order.Product.Price),
                productQuantityType: new ProductQuantityType(order.Product.QuantityType),
                productQuantity: new ProductQuantity(order.OrderItem.Quantity),
                totalPrice: new ProductPrice(order.OrderItem.Price * order.OrderItem.Quantity)
            )
            {
                OrderItemId = order.OrderItemId,
                OrderId = order.OrderItemId,
                ClientEmail = order.Client.Email,
                ProductId = order.Product.ProductId
            }).ToList();
        }

        public async Task UpdateOrderStateAsync(int orderId, OrderState newState)
        {
            var order = await dbContext.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }

            try
            {
                order.State = (int)newState;
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Unable to update the order state at this time.", ex);
            }
        }
        public async Task<List<int>> GetOrderIdsByEmailAsync(string email)
        {
            var orderIds = await (
                    from o in dbContext.Orders
                    where o.ClientEmail == email
                    select o.OrderId
                ).Distinct() // Pentru a evita duplicatele
                .ToListAsync();

            return orderIds;
        }
    }
}
