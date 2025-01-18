//using Microsoft.AspNetCore.Mvc;
//using PsscFinalProject.Domain.Models;
//using static PsscFinalProject.Domain.Models.OrderPublishEvent;

//[ApiController]
//[Route("api/[controller]")]
//public class OrdersController : ControllerBase
//{
//    private readonly PublishOrderWorkflow _workflow;
//    private readonly ILogger<OrdersController> _logger;

//    public OrdersController(PublishOrderWorkflow workflow, ILogger<OrdersController> logger)
//    {
//        _workflow = workflow;
//        _logger = logger;
//    }

//    /// <summary>
//    /// Adds an order and triggers the workflow.
//    /// </summary>
//    /// <param name="command">Order details.</param>
//    /// <returns>200 if successful, 400 for validation errors, 500 for internal server errors.</returns>
//    [HttpPost("add")] // Explicitly specify the HTTP method
//    public async Task<IActionResult> AddOrder([FromBody] PublishOrderCommand command)
//    {
//        try
//        {
//            if (command == null)
//            {
//                _logger.LogWarning("Invalid order data received.");
//                return BadRequest("Invalid order data.");
//            }

//            var result = await _workflow.ExecuteAsync(command);

//            if (result is OrderPublishSucceededEvent successEvent)
//            {
//                return Ok(new { Message = "Order created successfully.", successEvent.Csv });
//            }

//            if (result is OrderPublishFailedEvent failedEvent)
//            {
//                return BadRequest(new { Message = "Failed to create order.", failedEvent.Reasons });
//            }

//            return StatusCode(500, "Unexpected error occurred.");
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "An error occurred while creating the order.");
//            return StatusCode(500, "An internal error occurred.");
//        }
//    }

//    /// <summary>
//    /// Gets an order by ID.
//    /// </summary>
//    /// <param name="orderId">The ID of the order to retrieve.</param>
//    /// <returns>The requested order or 404 if not found.</returns>
//    //[HttpGet("{orderId}")] // Explicitly specify the HTTP method
//    //public async Task<IActionResult> GetOrder(int orderId)
//    //{
//    //    try
//    //    {
//    //        var order = await _workflow.GetOrderByIdAsync(orderId);
//    //        if (order == null)
//    //        {
//    //            return NotFound("Order not found.");
//    //        }

//    //        return Ok(order);
//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        _logger.LogError(ex, "An error occurred while retrieving the order.");
//    //        return StatusCode(500, "An internal error occurred.");
//    //    }
//    //}
//}
