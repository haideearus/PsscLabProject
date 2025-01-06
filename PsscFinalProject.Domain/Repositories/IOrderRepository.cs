using PsscFinalProject.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<List<OrderDto>> GetOrdersAsync();
        Task SaveOrdersAsync(IEnumerable<OrderDto> orders);
    }
}
