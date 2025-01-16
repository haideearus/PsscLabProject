using PsscFinalProject.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<List<ValidatedProduct>> GetProductsByCodesAsync(IEnumerable<string> productCodes);

        //Task<List<ValidatedProduct>> GetProductsByCodesAsync(IEnumerable<string> productCodes); // Fetch product details
        Task<List<ProductCode>> GetExistingProductsAsync(IEnumerable<string> productCodesToCheck); // Validate product codes
    }
}
