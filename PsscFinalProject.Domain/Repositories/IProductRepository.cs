using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<List<Products>> GetProductsByCodesAsync(IEnumerable<string> productCodes);
        Task<List<ProductQuantity>> GetProductStockByCodesAsync(IEnumerable<string> productCode);
        Task<List<ProductPrice>> GetProductPriceByCodesAsync(IEnumerable<string> productCode);
        Task<List<ProductCode>> GetExistingProductsAsync(IEnumerable<string> productCodesToCheck);
        Task<List<ProductUI>> GetAllProducts();
    }
}
