using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PsscFinalProject.Data;
using PsscFinalProject.Data.Models;

[Route("api/[controller]")]
[ApiController]
public class BillController : ControllerBase
{
    private readonly PsscDbContext _context;
    private readonly ILogger<BillController> _logger;

    public BillController(PsscDbContext context, ILogger<BillController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/bill
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BillDto>>> GetAllBills()
    {
        try
        {
            var bills = await _context.Bills.ToListAsync();
            return Ok(bills);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching bills.");
            return StatusCode(500, "Internal server error.");
        }
    }

    // GET: api/bill/{id}
    [HttpGet("order/{id}")]  
    public async Task<IActionResult> GetBillByOrderId(int id)  
    {
        try
        {
            var bill = await _context.Bills
                .Where(b => b.OrderId == id) 
                .FirstOrDefaultAsync(); 

            if (bill == null)
            {
                return NotFound($"Bill associated with Order ID {id} not found.");
            }

            return Ok(bill);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching the bill for Order ID {OrderId}.", id);
            return StatusCode(500, "Internal server error.");
        }
    }
}