
using PsscFinalProject.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IBillRepository
    {
        Task<List<BillDto>> GetBillsAsync();
        Task SaveBillsAsync(IEnumerable<BillDto> bills);
    }
}
