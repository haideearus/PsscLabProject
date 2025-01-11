using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.Order;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<List<CalculatedOrder>> GetExistingOrdersAsync();

        Task SaveOrdersAsync(PaidOrder paidOrder);
    }
}
