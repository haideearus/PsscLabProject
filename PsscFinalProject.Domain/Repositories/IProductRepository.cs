using PsscFinalProject.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<List<ProductDto>> GetProductsAsync();
        Task SaveProductsAsync(IEnumerable<ProductDto> products);
    }
}
