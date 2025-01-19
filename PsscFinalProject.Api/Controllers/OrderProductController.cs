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
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using PsscFinalProject.Api.Models;
using static PsscFinalProject.Domain.Models.OrderBilling;
using PsscFinalProject.Data.Repositories;
using static PsscFinalProject.Domain.Models.OrderDelivery;

namespace PsscFinalProject.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientOrderController : ControllerBase
    {

        private readonly ILogger<ClientOrderController> logger;
        private readonly PublishOrderWorkflow publishOrderWorkflow;
        private readonly PublishBillingWorkflow publishBillingWorkflow;
        private readonly PublishDeliveryWorkflow publishDeliveryWorkflow;
        private readonly IOrderRepository orderRepository; // Add this line

        // Modify the constructor to accept IOrderRepository
        public ClientOrderController(
            ILogger<ClientOrderController> logger,
            PublishOrderWorkflow publishOrderWorkflow,
            PublishBillingWorkflow publishBillingWorkflow,
            IOrderRepository orderRepository,
            PublishDeliveryWorkflow publishDeliveryWorkflow) // Add this line
        {
            this.logger = logger;
            this.publishOrderWorkflow = publishOrderWorkflow;
            this.publishBillingWorkflow = publishBillingWorkflow;
            this.orderRepository = orderRepository; // Assign the injected repository
            this.publishDeliveryWorkflow = publishDeliveryWorkflow;
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

                switch (result)
                {
                    case OrderPublishSucceededEvent success:
                        // Retrieve the existing orders from the database
                        var existingOrders = await orderRepository.GetExistingOrdersAsync();

                        // Find the order matching the client's email
                        var existingOrder = existingOrders.FirstOrDefault(o => o.clientEmail.Value == success.ClientEmail.Value);

                        if (existingOrder == null)
                        {
                            return NotFound($"Order for client {success.ClientEmail} not found.");
                        }

                        // Extract the shipping address
                        var shippingAddress = "Oras Arad, Strada Garii, nr. 124";

                        // Generate a valid bill number
                        var billNumber = BillNumber.Generate();

                        // Create unvalidated bills from the successful order
                        var unvalidatedBilling = new UnvalidatedOrderBilling(new List<UnvalidatedBill>
                {
                    new UnvalidatedBill(billNumber.Value, shippingAddress, success.ProductPrice.Value)
                }.AsReadOnly());

                        // Trigger the billing workflow
                        var billingResult = await publishBillingWorkflow.ExecuteAsync(unvalidatedBilling, new PaidOrderProducts(
                            productsList: new List<CalculatedProduct> { existingOrder }, // Wrapping existing order in a list
                            totalAmount: success.ProductPrice,
                            csv: success.Csv,
                            orderDate: success.PublishDate,
                            clientEmail: success.ClientEmail
                        ));

                        if (billingResult is not PublishedOrderBilling publishedBilling)
                        {
                            return BadRequest(new
                            {
                                Message = "Order placed, but billing failed.",
                                BillingStatus = "Billing failed."
                            });
                        }

                        // Use the same OrderId for the delivery workflow
                        var unvalidatedDelivery = new UnvalidatedOrderDelivery(new List<UnvalidatedDelivery>
                {
                    new UnvalidatedDelivery(
                        TrackingNumber.Generate().Value,
                        "Sameday" // Example courier, this can be dynamic or user-provided
                    )
                }.AsReadOnly());

                        // Trigger the delivery workflow
                        var deliveryResult = await publishDeliveryWorkflow.ExecuteAsync(unvalidatedDelivery, new PaidOrderProducts(
                            productsList: new List<CalculatedProduct> { existingOrder }, // Wrapping existing order in a list
                            totalAmount: success.ProductPrice,
                            csv: success.Csv,
                            orderDate: success.PublishDate,
                            clientEmail: success.ClientEmail
                        ), existingOrder.OrderId); // Pass OrderId

                        return Ok(new
                        {
                            Message = "Order placed successfully.",
                            BillingStatus = "Billing succeeded.",
                            DeliveryStatus = deliveryResult is PublishedOrderDelivery ? "Delivery succeeded." : "Delivery failed."
                        });

                    case OrderPublishFailedEvent failure:
                        return BadRequest(new
                        {
                            Errors = failure.Reasons
                        });

                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "Unknown result.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while placing the order.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while placing the order.");
            }
        }

    }
}
