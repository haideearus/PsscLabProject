using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PsscFinalProject.Domain.Models;
using System;
using PsscFinalProject.Data.Models;
using PsscFinalProject.Data;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly PsscDbContext _context;

    public ProductsController(PsscDbContext context)
    {
        _context = context;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }

    // POST: api/products/productCode
    [HttpPost("search")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts([FromBody] string searchTerm)
    {
        var products = await _context.Products
                                      .Where(p => p.Name.Contains(searchTerm) || p.Code.Contains(searchTerm))
                                      .ToListAsync();

        if (!products.Any())
        {
            return NotFound("No products found");
        }

        return Ok(products);
    }

    // PATCH: api/products/{productCode}/add-stock
    [HttpPatch("{productCode}/add-stock")]
    public async Task<IActionResult> AddStock(string productCode, [FromBody] int additionalStock)
    {
        if (additionalStock <= 0)
        {
            return BadRequest("The stock to add must be greater than zero.");
        }

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Code == productCode);

        if (product == null)
        {
            return NotFound($"Product with code {productCode} not found.");
        }

        // Add the additional stock
        product.Stock += additionalStock;

        // Save changes to the database
        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = $"Stock updated successfully for product {productCode}.",
            UpdatedStock = product.Stock
        });
    }

    // DELETE: api/products/{productCode}
    [HttpDelete("{productCode}")]
    public async Task<IActionResult> DeleteProduct(string productCode)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Code == productCode);

        if (product == null)
        {
            return NotFound($"Product with code '{productCode}' not found.");
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent(); 
    }

}