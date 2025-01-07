using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<List<OrderDto>> GetOrdersAsync();
        public interface IOrderRepository
        {
            Task SaveAsync(Order order);
        }

        Task SaveOrdersAsync(IEnumerable<OrderDto> orders);
        Task SaveAsync(Order order);
        Task SaveOrderAsync(Order order);
    }
}
