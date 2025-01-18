//using PsscFinalProject.Data.Models;
//using PsscFinalProject.Domain.Models;

//public interface IOrderService
//{
//    /// <summary>
//    /// Adds a new order and its items to the database.
//    /// </summary>
//    /// <param name="order">The order to add.</param>
//    /// <returns>The ID of the newly added order.</returns>
//    Task<int> AddOrderAsync(OrderDto order);

//    /// <summary>
//    /// Retrieves an order by its ID.
//    /// </summary>
//    /// <param name="orderId">The ID of the order to retrieve.</param>
//    /// <returns>The order with its items, or null if not found.</returns>
//    Task<OrderDto?> GetOrderByIdAsync(int orderId);

//    /// <summary>
//    /// Retrieves all orders for a specific client.
//    /// </summary>
//    /// <param name="clientEmail">The email of the client.</param>
//    /// <returns>A list of orders for the client.</returns>
//    Task<IEnumerable<OrderDto>> GetOrdersByClientEmailAsync(string clientEmail);

//    /// <summary>
//    /// Updates an existing order (e.g., state or shipping address).
//    /// </summary>
//    /// <param name="order">The updated order details.</param>
//    /// <returns>True if the update was successful, false otherwise.</returns>
//    Task<bool> UpdateOrderAsync(OrderDto order);

//    /// <summary>
//    /// Deletes an order by its ID.
//    /// </summary>
//    /// <param name="orderId">The ID of the order to delete.</param>
//    /// <returns>True if the deletion was successful, false otherwise.</returns>
//    Task<bool> DeleteOrderAsync(int orderId);
//}
