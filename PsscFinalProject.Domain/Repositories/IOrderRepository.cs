using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.OrderProducts;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<List<CalculatedProduct>> GetExistingOrdersAsync();
        Task SaveOrdersAsync(PaidOrderProducts paidOrder);
        Task<PaidOrderProducts> GetOrderByIdAsync(int orderId);
        Task<List<CalculatedProduct>> GetOrdersByEmailAsync(string email);
        Task UpdateOrderStateAsync(int orderId, OrderState newState);
        Task<List<int>> GetOrderIdsByEmailAsync(string email);
    }
}
