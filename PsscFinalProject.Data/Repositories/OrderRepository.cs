using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsscFinalProject.Domain.Repositories;

namespace PsscFinalProject.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly PsscDbContext dbContext;

        public OrderRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<OrderDto>> GetOrdersAsync()
        {
            return await dbContext.Orders.AsNoTracking().ToListAsync();
        }

        public async Task SaveOrdersAsync(IEnumerable<OrderDto> orders)
        {
            dbContext.Orders.AddRange(orders.Where(o => o.OrderId == 0));
            dbContext.Orders.UpdateRange(orders.Where(o => o.OrderId > 0));
            await dbContext.SaveChangesAsync();
        }
    }
}
