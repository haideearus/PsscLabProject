using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IProductRepository
    {

        Task<List<ProductCode>> GetExistingProductsAsync(IEnumerable<string> productsToCheck);
    }
}
