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

        // Retrieve all bills associated with their orders
        public async Task<List<CalculatedBillNumber>> GetAll()
        {
            // Load bills and join them with orders
            var bills = await (
                from b in dbContext.Bills
                join o in dbContext.Orders on b.OrderId equals o.OrderId
                select new
                {
                    b.BillId,
                    b.BillNumber,
                    o.ShippingAddress,
                    o.TotalAmount
                }
            ).ToListAsync();

            // Map results to the domain model
            var calculatedBills = bills.Select(result => new CalculatedBillNumber(
                ShippingAddress: new(result.ShippingAddress),    // Order's address
                BillNumber: new(result.BillNumber),              // Bill number
                ProductPrice: new(result.TotalAmount)             // Order's total amount
            )
            {
                BillId = result.BillId // Bill ID
            }).ToList();

            return calculatedBills;
        }

        // Save new bills and associate them with existing orders
        // Save new bills and associate them with the most recently added order
        public async Task SaveBills(PublishedOrderBilling bills)
        {
            // Retrieve the most recently added order
            var mostRecentOrder = await dbContext.Orders
                .OrderByDescending(o => o.OrderDate) // Assuming OrderDate or equivalent timestamp field exists
                .FirstOrDefaultAsync();

            if (mostRecentOrder == null)
            {
                throw new InvalidOperationException("No orders found in the database to associate with the bills.");
            }

            // Iterate through the bills and associate them with the most recent order
            foreach (var bill in bills.BillList)
            {
                // Create a new Bill entity
                var newBill = new BillDto
                {
                    OrderId = mostRecentOrder.OrderId, // Use the most recent order's ID
                    BillNumber = BillNumber.Generate().Value, // Generate a unique bill number
                    BillingDate = DateTime.Now,
                    TotalAmount = mostRecentOrder.TotalAmount // Use the total amount from the most recent order
                };

                await dbContext.Bills.AddAsync(newBill);
            }

            // Save changes to the database
            await dbContext.SaveChangesAsync();
        }
        public async Task<List<Bill>> GetAllBy(List<int> orderIds)
        {
            // Load bills and join them with orders
            var billDto = await dbContext.Bills
                .Where(d => orderIds.Contains(d.OrderId)) // Filtrăm după lista de orderId-uri
                .AsNoTracking()
                .ToListAsync();
            var bills = billDto.Select(dto => new Bill
            {
                BillId = dto.BillId,
                OrderId = dto.OrderId,
                BillDateTime = dto.BillingDate,
                BillNumber = dto.BillNumber,
                Amount = dto.TotalAmount,
                // Adaugă alte câmpuri necesare aici
            }).ToList();

            return bills;
        }

    }
}
