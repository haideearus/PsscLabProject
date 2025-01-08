using Microsoft.AspNetCore.Mvc;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Workflows;

namespace PsscFinalProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly BillOrderWorkflow _billWorkflow;

        public BillController(BillOrderWorkflow billWorkflow)
        {
            _billWorkflow = billWorkflow;
        }

        // GET: api/bills
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Bill>>> GetBills()
        //{
        //    var bills = await _billWorkflow.GetBillsAsync();
        //    return Ok(bills);
        //}

        // GET: api/bills/{id}
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Bill>> GetBill(int id)
        //{
        //    var bill = await _billWorkflow.GetBillByIdAsync(id);
        //    if (bill == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(bill);
        //}

        //// POST: api/bills
        //[HttpPost]
        //public async Task<IActionResult> SaveBills([FromBody] List<Bill> bills)
        //{
        //    await _billWorkflow.SaveBillsAsync(bills);
        //    return CreatedAtAction(nameof(GetBills), new { }, bills);
        //}
    }
}