
using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.OrderDelivery;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IDeliveryRepository
    {
       Task<List<CalculatedTrackingNumber>> GetExistingDeliveriesAsync();
       Task SaveDeliveries(PublishedOrderDelivery deliveries);

       Task<List<Delivery>> GetAllDeliveries(List<int> orderIds);
    }
}
