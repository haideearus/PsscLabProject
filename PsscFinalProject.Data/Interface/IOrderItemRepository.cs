using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Data.Interface
{
    public interface IOrderItemRepository
    {
        Task AddOrderItemsAsync(IEnumerable<OrderItem> orderItems);
        Task AddOrderItemsAsync1(IEnumerable<OrderItem> orderItems);
        Task SaveOrderItemsAsync(IEnumerable<ValidatedProduct> orderItems);
        Task DeleteOrderItemAsync(int orderItemId);
        Task AddOrderItemAsync(int orderId, string productCode, decimal price);
        Task AddOrderItemsAsync(IEnumerable<ValidatedProduct> orderItems);

    }
}
