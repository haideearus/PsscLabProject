using Microsoft.EntityFrameworkCore;
using PsscFinalProject.Data;
using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Repositories;

public class ProductRepository : IProductRepository
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
