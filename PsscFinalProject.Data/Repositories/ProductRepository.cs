using PsscFinalProject.Domain.Models;
using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsscFinalProject.Data.Interface;

namespace PsscFinalProject.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly PsscDbContext dbContext;

        public ProductRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<ValidatedProduct>> GetProductsByCodesAsync(IEnumerable<string> productCodes)
        {
            var products = await dbContext.Products
                .Where(p => productCodes.Contains(p.Code))
                .AsNoTracking()
                .ToListAsync();

            return products.Select(p => new ValidatedProduct(
                ProductName.Create(p.Name),
                ProductCode.Create(p.Code),
                ProductPrice.Create(p.Price),
                ProductQuantityType.Create(p.QuantityType),
                ProductQuantity.Create(1) // Default quantity
            )).ToList();
        }
        public async Task<List<ProductCode>> GetExistingProductsAsync(IEnumerable<string> productCodesToCheck)
        {
            var products = await dbContext.Products
                .Where(product => productCodesToCheck.Contains(product.Code))
                .AsNoTracking()
                .ToListAsync();

            return products.Select(product => new ProductCode(product.Code)).ToList();
        }
    }
}
