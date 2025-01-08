//using Microsoft.AspNetCore.Mvc;
//using PsscFinalProject.Domain.Models;
//using PsscFinalProject.Domain.Workflows;
//using System.Threading.Tasks;

//namespace PsscFinalProject.Api.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class OrdersController : ControllerBase
//    {
//        private readonly TakeOrderWorkflow takeOrderWorkflow;

//        public OrdersController(TakeOrderWorkflow takeOrderWorkflow)
//        {
//            this.takeOrderWorkflow = takeOrderWorkflow;
//        }

//        [HttpPost("take-order")]
//        public async Task<IActionResult> TakeOrder([FromBody] TakeOrderCommand command)
//        {
//            var result = await takeOrderWorkflow.ExecuteAsync(command);

//            if (result is OrderProcessedEvent success)
//            {
//                return Ok(new { success.OrderId, success.Message });
//            }
//            else if (result is OrderProcessingFailedEvent failure)
//            {
//                return BadRequest(new { failure.Reason });
//            }

//            return StatusCode(500, "An unexpected error occurred.");
//        }
//    }
//}
