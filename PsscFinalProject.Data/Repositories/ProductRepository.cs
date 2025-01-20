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
            List<ProductDto> products = await dbContext.Products
                .Where(product => productCodesToCheck.Contains(product.Code))
                .AsNoTracking()
                .ToListAsync();

            return products.Select(product => new ProductCode(product.Code)).ToList();
        }

        public async Task<List<Products>> GetProductsByCodesAsync(IEnumerable<string> productCodes)
        {
            List<ProductDto> catalog = await dbContext.Products
                .Where(product => productCodes.Contains(product.Code))
                .AsNoTracking()
                .ToListAsync();
            return catalog.Select(
                 product => new Products(new ProductCode(product.Code),
                new ProductQuantity(product.Stock), new ProductPrice(product.Price)))
                    .ToList();
        }

        public async Task<List<ProductQuantity>> GetProductStockByCodesAsync(IEnumerable<string> productCode)
        {
            List<ProductDto> catalog = await dbContext.Products
                .Where(product => productCode.Contains(product.Code))
                .AsNoTracking()
                .ToListAsync();
            return catalog.Select(
                product => new ProductQuantity(product.Stock))
                .ToList();
        }

        public async Task<List<ProductPrice>> GetProductPriceByCodesAsync(IEnumerable<string> productCode)
        {
            List<ProductDto> catalog = await dbContext.Products
                           .Where(product => productCode.Contains(product.Code))
                           .AsNoTracking()
                           .ToListAsync();
            return catalog.Select(
                product => new ProductPrice(product.Price))
                            .ToList();
        }

        public async Task<List<ProductUI>> GetAllProducts()
        {
            var ProductsDtos= await dbContext.Products.AsNoTracking().ToListAsync();

            var products = ProductsDtos.Select(dto => new ProductUI
            {
                ProductName = dto.Name,
                ProductCode = dto.Code,
                ProductPrice = dto.Price,
                ProductQuantity = dto.Stock,
                ProductQuantityType = dto.QuantityType

            }).ToList();
            return products;
        }
        
    }
}
