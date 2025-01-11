using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsscFinalProject.Domain.Repositories;

namespace PsscFinalProject.Data.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly PsscDbContext dbContext;

        public BillRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<BillDto>> GetBillsAsync()
        {
            return await dbContext.Bills.AsNoTracking().ToListAsync();
        }

        public async Task SaveBillsAsync(IEnumerable<BillDto> bills)
        {
            dbContext.Bills.AddRange(bills.Where(b => b.BillId == 0));
            dbContext.Bills.UpdateRange(bills.Where(b => b.BillId > 0));
            await dbContext.SaveChangesAsync();
        }
    }
}