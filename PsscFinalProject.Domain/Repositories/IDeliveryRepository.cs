using PsscFinalProject.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IDeliveryRepository
    {
        Task<List<DeliveryDto>> GetDeliveriesAsync();
        Task SaveDeliveriesAsync(IEnumerable<DeliveryDto> deliveries);
    }
}
