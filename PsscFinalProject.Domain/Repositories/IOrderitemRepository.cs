using PsscFinalProject.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IOrderItemRepository
    {
        Task<List<OrderitemDto>> GetOrderItemsAsync();
        Task SaveOrderItemsAsync(IEnumerable<OrderitemDto> orderItems);
    }
}
