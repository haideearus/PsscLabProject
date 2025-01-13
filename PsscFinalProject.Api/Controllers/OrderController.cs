using Microsoft.AspNetCore.Mvc;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Workflows;
using static PsscFinalProject.Domain.Models.OrderPublishEvent;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace PsscFinalProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly PublishOrderWorkflow _workflow;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(PublishOrderWorkflow workflow, ILogger<OrdersController> logger)
        {
            _workflow = workflow;
            _logger = logger;
        }

        /// <summary>
        /// Publishes orders and returns the result.
        /// </summary>
        /// <param name="command">The command containing a collection of unvalidated orders to be processed.</param>
        /// <returns>
        /// 200 - Orders successfully published
        /// 400 - Invalid data or failed to publish orders
        /// 500 - Server error
        /// </returns>
        [HttpPost("publish")]
        public async Task<IActionResult> PublishOrder([FromBody] PublishOrderCommand command)
        {
            try
            {
                // Check if the incoming command is valid
                if (command == null || command.InputOrders == null || !command.InputOrders.Any())
                {
                    _logger.LogWarning("Invalid order data provided.");
                    return BadRequest("Invalid order data.");
                }

                // Validate the orders (e.g., using model validation)
                foreach (var inputOrder in command.InputOrders)
                {
                    var validationResults = new List<ValidationResult>();
                    var validationContext = new ValidationContext(inputOrder);
                    if (!Validator.TryValidateObject(inputOrder, validationContext, validationResults, true))
                    {
                        var errorMessages = validationResults.Select(r => r.ErrorMessage).ToList();
                        _logger.LogWarning("Validation failed for input order: {Errors}", string.Join(", ", errorMessages));
                        return BadRequest(new { Errors = errorMessages });
                    }
                }

                // Execute the workflow to process and publish the orders
                var result = await _workflow.ExecuteAsync(command);

                // If the result is a failure event, return the error details
                if (result is OrderPublishFailedEvent failedEvent)
                {
                    _logger.LogWarning("Order publishing failed: {Errors}", string.Join(", ", failedEvent.Reasons));
                    return BadRequest(new { Errors = failedEvent.Reasons });
                }

                // Log success and return the result (success event)
                _logger.LogInformation("Orders published successfully.");
                return Ok(new
                {
                    Message = "Orders published successfully.",
                    Csv = ((OrderPublishSucceededEvent)result).Csv,
                    PublishedDate = ((OrderPublishSucceededEvent)result).PublishedDate,
                    OrderDetails = ((OrderPublishSucceededEvent)result).OrderDetails
                });
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic server error response
                _logger.LogError(ex, "An error occurred while publishing the order.");
                return StatusCode(500, new { Message = "An error occurred while processing the request.", Details = ex.Message });
            }
        }
    }
}