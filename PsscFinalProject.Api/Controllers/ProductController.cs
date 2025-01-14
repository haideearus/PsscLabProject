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

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {

        var product = await _context.Products.FindAsync(id);
        
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    // POST: api/products/search
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

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}