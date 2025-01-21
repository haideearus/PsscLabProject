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


        public async Task UpdateStockAsync(ProductCode productCode, ProductQuantity quantity)
        {
            var productDto = await dbContext.Products.FirstOrDefaultAsync(p => p.Code == productCode.Value);

            if (productDto == null)
            {
                throw new InvalidOperationException($"Product with code {productCode.Value} not found.");
            }

            if (productDto.Stock < quantity.Value)
            {
                throw new InvalidOperationException($"Insufficient stock for product {productCode.Value}. Available: {productDto.Stock}, Requested: {quantity.Value}.");
            }

            // Update stock directly on the ProductDto
            productDto.Stock -= quantity.Value;

            // Save the changes to the database
            dbContext.Products.Update(productDto);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<ValidatedProduct>> GetAllProducts()
        {
            // Step 1: Fetch products from the database
            var productDtos = await dbContext.Products.AsNoTracking().ToListAsync();

            // Step 2: Transform to Unvalidated Products using the constructor
            var unvalidatedProducts = productDtos.Select(dto => new UnvalidatedProduct(
                new ProductName(dto.Name).Value,                          
                new ProductCode(dto.Code).Value,                          
                new ProductPrice(dto.Price).Value,                   
                new ProductQuantityType(dto.QuantityType).Value, 
                new ProductQuantity(dto.Stock).Value        
            )).ToList();

            // Step 3: Validate and transform to Validated Products
            var validatedProducts = unvalidatedProducts.Select(up =>
                new ValidatedProduct(
                    new ProductName(up.ProductName),
                    new ProductCode(up.ProductCode),
                    new ProductPrice(up.ProductPrice.Value),
                    new ProductQuantityType(up.ProductQuantityType),
                    new ProductQuantity(up.ProductQuantity.Value)
                )).ToList();

            return validatedProducts;
        }

    }
}
