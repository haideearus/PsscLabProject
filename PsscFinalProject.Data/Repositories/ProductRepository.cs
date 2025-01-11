using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsscFinalProject.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly PsscDbContext dbContext;

        public ProductRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<ProductCode>> GetExistingProductsAsync(IEnumerable<string> productCodesToCheck)
        {
            // Fetch products based on the provided product codes
            List<ProductDto> products = await dbContext.Products
                .Where(product => productCodesToCheck.Contains(product.Code))
                .AsNoTracking()
                .ToListAsync();

            // Map the fetched products to domain models (ProductCode)
            return products.Select(product => new ProductCode(product.Code))
                           .ToList();
        }
    }
}
