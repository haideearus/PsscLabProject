using Microsoft.AspNetCore.Mvc;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Workflows;
using static PsscFinalProject.Domain.Models.OrderPublishEvent;

namespace PsscFinalProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly PublishOrderWorkflow _workflow;

        public OrdersController(PublishOrderWorkflow workflow)
        {
            _workflow = workflow;
        }

        [HttpPost("publish")]
        public async Task<IActionResult> PublishOrder([FromBody] PublishOrderCommand command)
        {
            if (command == null || command.InputOrders == null || !command.InputOrders.Any())
            {
                return BadRequest("Invalid order data.");
            }

            var result = await _workflow.ExecuteAsync(command);

            if (result is OrderPublishFailedEvent failedEvent)
            {
                return BadRequest(new { Errors = failedEvent.Reasons });
            }

            return Ok(result);
        }
    }
}
