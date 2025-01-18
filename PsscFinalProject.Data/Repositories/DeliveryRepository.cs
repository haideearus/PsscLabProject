using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsscFinalProject.Data.Interface;

namespace PsscFinalProject.Data.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly PsscDbContext dbContext;

        public DeliveryRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<DeliveryDto>> GetDeliveriesAsync()
        {
            return await dbContext.Deliveries.AsNoTracking().ToListAsync();
        }

        public async Task SaveDeliveriesAsync(IEnumerable<DeliveryDto> deliveries)
        {
            dbContext.Deliveries.AddRange(deliveries.Where(d => d.DeliveryId == 0));
            dbContext.Deliveries.UpdateRange(deliveries.Where(d => d.DeliveryId > 0));
            await dbContext.SaveChangesAsync();
        }
    }
}
