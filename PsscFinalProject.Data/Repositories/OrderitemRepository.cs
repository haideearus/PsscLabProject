using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsscFinalProject.Data.Repositories
{
    public class OrderItemRepository
    {
        private readonly PsscDbContext dbContext;

        public OrderItemRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<OrderitemDto>> GetOrderItemsAsync()
        {
            return await dbContext.Orderitems.AsNoTracking().ToListAsync();
        }

        public async Task SaveOrderItemsAsync(IEnumerable<OrderitemDto> orderItems)
        {
            dbContext.Orderitems.AddRange(orderItems.Where(o => o.OrderItemId == 0));
            dbContext.Orderitems.UpdateRange(orderItems.Where(o => o.OrderItemId > 0));
            await dbContext.SaveChangesAsync();
        }
    }
}
