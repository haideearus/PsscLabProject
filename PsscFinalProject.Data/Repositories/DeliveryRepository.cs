using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using PsscFinalProject.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<CalculatedTrackingNumber>> GetExistingDeliveriesAsync()
        {
            // Query to join Deliveries with Orders based on OrderId
            var deliveries = await (
                from d in dbContext.Deliveries
                join o in dbContext.Orders on d.OrderId equals o.OrderId
                select new
                {
                    d.DeliveryId,
                    d.OrderId,
                    d.TrackingNumber,
                    d.Courier,
                    o.ShippingAddress
                }
            ).AsNoTracking().ToListAsync();

            // Map the results to the domain model
            return deliveries.Select(result => new CalculatedTrackingNumber(
                TrackingNumber: new TrackingNumber(result.TrackingNumber),
                Courier: new Courier(result.Courier)
                //ShippingAddress: new ShippingAddress(result.ShippingAddress),
            )
            {
                DeliveryId = result.DeliveryId
            }).ToList();
        }

        public async Task SaveDeliveries(PublishedOrderDelivery deliveries)
        {
            // Fetch the latest OrderId from the Orders table
            var lastOrderId = await dbContext.Orders
                .OrderByDescending(o => o.OrderDate) // Assuming OrderDate is available
                .Select(o => o.OrderId)
                .FirstOrDefaultAsync();

            if (lastOrderId == 0)
            {
                throw new InvalidOperationException("No orders found in the database.");
            }

            foreach (var delivery in deliveries.DeliveryList)
            {
                var deliveryEntity = new DeliveryDto
                {
                    TrackingNumber = delivery.TrackingNumber.Value,
                    Courier = delivery.Courier.Value,
                    DeliveryDate = DateTime.UtcNow,
                    OrderId = lastOrderId // Use the last OrderId fetched from the Orders table
                };

                await dbContext.Deliveries.AddAsync(deliveryEntity);
            }

            await dbContext.SaveChangesAsync();
        }





    }
}
