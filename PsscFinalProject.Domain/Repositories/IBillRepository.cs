
using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.OrderBilling;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IBillRepository
    {
        Task<List<CalculatedBillNumber>> GetAll();
        Task SaveBills(PublishedOrderBilling bills);
    }
}