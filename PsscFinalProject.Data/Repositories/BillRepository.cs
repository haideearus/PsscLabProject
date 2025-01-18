using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsscFinalProject.Domain.Repositories;
using PsscFinalProject.Domain.Models;
using static PsscFinalProject.Domain.Models.OrderBilling;

namespace PsscFinalProject.Data.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly PsscDbContext dbContext;

        public BillRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

       public async Task<List<CalculatedBillNumber>> GetAll()
        {
            await dbContext.SaveChangesAsync();
            return new List<CalculatedBillNumber>(); //fake doar sa nu am eroare 
        }

        
        public async Task SaveBills(PublishedOrderBilling bills)
        {
            await dbContext.SaveChangesAsync();//provizoriu
        }
    }
}