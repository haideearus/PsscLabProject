using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsscFinalProject.Domain.Repositories;
using PsscFinalProject.Domain.Models;
using static PsscFinalProject.Domain.Models.OrderDelivery;

namespace PsscFinalProject.Data.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly PsscDbContext dbContext;

        public DeliveryRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<CalculatedTrackingNumber>> GetDeliveriesAsync()
        {
            await dbContext.SaveChangesAsync();
            return new List<CalculatedTrackingNumber>(); //fake doar sa nu am eroare 
        }

        public async Task SaveDeliveries(PublishedOrderDelivery deliveries)
        {
          
            await dbContext.SaveChangesAsync();
        }
    }
}
