using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Workflows;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.OrderPublishEvent;

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
        /// Adds an order and triggers the workflow.
        /// </summary>
        /// <param name="command">Order details.</param>
        /// <returns>
        /// 200 - Order processed successfully.
        /// 400 - Validation errors or workflow failure.
        /// 500 - Internal server error.
        /// </returns>
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddOrder([FromBody] PublishOrderCommand command)
        {
            try
            {
                // Validate the incoming command
                if (command == null || command.ProductList == null || !command.ProductList.Any())
                {
                    _logger.LogWarning("Invalid order data received.");
                    return BadRequest("Invalid order data.");
                }

                // Execute the workflow
                var result = await _workflow.ExecuteAsync(command);

                // Check the result type
                if (result is OrderPublishSucceededEvent successEvent)
                {
                    return Ok(new
                    {
                        Message = "Order processed successfully.",
                        Csv = successEvent.Csv,
                        OrderDetails = successEvent.OrderDetails
                    });
                }

                if (result is OrderPublishFailedEvent failedEvent)
                {
                    return BadRequest(new
                    {
                        Message = "Failed to process the order.",
                        Errors = failedEvent.Reasons
                    });
                }

                // Unexpected result type
                return StatusCode(500, "Unexpected error occurred.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the order.");
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
    }
}
