using PsscFinalProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsscFinalProject.Data.Repositories
{
    public class ProductRepository
    {
        private readonly PsscDbContext dbContext;

        public ProductRepository(PsscDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            return await dbContext.Products.AsNoTracking().ToListAsync();
        }

        public async Task SaveProductsAsync(IEnumerable<ProductDto> products)
        {
            dbContext.Products.AddRange(products.Where(p => p.ProductId == 0));
            dbContext.Products.UpdateRange(products.Where(p => p.ProductId > 0));
            await dbContext.SaveChangesAsync();
        }
    }
}
