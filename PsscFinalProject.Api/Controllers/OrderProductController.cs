using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PsscFinalProject.Domain.Repositories;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Operations;
using static PsscFinalProject.Domain.Models.OrderProducts;
using static PsscFinalProject.Domain.Models.OrderPublishEvent;

namespace PsscFinalProject.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientOrderController : ControllerBase
    {
        private readonly ILogger<ClientOrderController> logger;
        private readonly PublishOrderWorkflow publishOrderWorkflow;

        public ClientOrderController(ILogger<ClientOrderController> logger, PublishOrderWorkflow publishOrderWorkflow)
        {
            this.logger = logger;
            this.publishOrderWorkflow = publishOrderWorkflow;
        }

        [HttpGet("getAllOrders")]
        public async Task<IActionResult> GetAllOrders([FromServices] IOrderRepository orderRepository)
        {
            try
            {
                var orders = await orderRepository.GetExistingOrdersAsync();

                return Ok(orders.Select(order => new
                {
                    ClientEmail = order.clientEmail.Value,
                    ProductCode = order.productCode.Value,
                    ProductPrice = order.productPrice.Value,
                    Quantity = order.productQuantity.Value,
                    TotalPrice = order.totalPrice.Value
                }));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching orders.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching orders.");
            }
        }

        [HttpPost("placeOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] IEnumerable<UnvalidatedOrder> unvalidatedOrders)
        {
            try
            {
                var command = new PublishOrderCommand(unvalidatedOrders.ToList().AsReadOnly());
                var result = await publishOrderWorkflow.ExecuteAsync(command);

                return result switch
                {
                    OrderPublishSucceededEvent success => Ok(new
                    {
                        success.ClientEmail,
                        success.Csv,
                        success.ProductPrice.Value,
                        success.PublishDate
                    }),
                    OrderPublishFailedEvent failure => BadRequest(new
                    {
                        Errors = failure.Reasons
                    }),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, "Unknown result.")
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while placing the order.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while placing the order.");
            }
        }
    }
}
