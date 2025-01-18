//using Microsoft.EntityFrameworkCore;
//using PsscFinalProject.Data;
//using PsscFinalProject.Data.Models;
//using PsscFinalProject.Domain.Models;

//public class OrderService : IOrderService
//{
//    private readonly PsscDbContext _context;

//    public OrderService(PsscDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<int> AddOrderAsync(OrderDto order)
//    {
//        if (order == null)
//        {
//            throw new ArgumentNullException(nameof(order), "Order cannot be null.");
//        }

//        // Add the order
//        _context.Orders.Add(order);

//        // Add related order items
//        foreach (var item in order.OrderItems)
//        {
//            item.OrderItemId = order.OrderId; // Ensure Foreign Key is linked
//            _context.OrderItems.Add(item);
//        }

//        await _context.SaveChangesAsync();
//        return order.OrderId; // Return the generated OrderId
//    }


//    public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
//    {
//        return await _context.Orders
//            .Include(o => o.OrderItems) // Include related items
//            .FirstOrDefaultAsync(o => o.OrderId == orderId);
//    }



//    public async Task<IEnumerable<OrderDto>> GetOrdersByClientEmailAsync(string clientEmail)
//    {
//        return await _context.Orders
//            .Where(o => o.ClientEmail == clientEmail)
//            .Include(o => o.OrderItems) // Include items in each order
//            .ToListAsync();
//    }

//    public async Task<bool> UpdateOrderAsync(OrderDto order)
//    {
//        var existingOrder = await _context.Orders
//            .Include(o => o.OrderItems)
//            .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

//        if (existingOrder == null)
//        {
//            return false;
//        }

//        // Update order fields
//        existingOrder.ShippingAddress = order.ShippingAddress;
//        existingOrder.State = order.State;
//        existingOrder.TotalAmount = order.TotalAmount;

//        // Update order items
//        foreach (var updatedItem in order.OrderItems)
//        {
//            var existingItem = existingOrder.OrderItems
//                .FirstOrDefault(i => i.ProductCode == updatedItem.ProductCode);

//            if (existingItem != null)
//            {
//                // Update existing item
//                existingItem.Price = updatedItem.Price;
//                existingItem.Quantity = updatedItem.Quantity;
//            }
//            else
//            {
//                // Add new item
//                updatedItem.OrderItemId = existingOrder.OrderId;
//                _context.OrderItems.Add(updatedItem);
//            }
//        }

//        await _context.SaveChangesAsync();
//        return true;
//    }

//    public async Task<bool> DeleteOrderAsync(int orderId)
//    {
//        var order = await _context.Orders
//            .Include(o => o.OrderItems)
//            .FirstOrDefaultAsync(o => o.OrderId == orderId);

//        if (order == null)
//        {
//            return false;
//        }

//        _context.OrderItems.RemoveRange(order.OrderItems); // Remove related items
//        _context.Orders.Remove(order); // Remove the order

//        await _context.SaveChangesAsync();
//        return true;
//    }

//}
