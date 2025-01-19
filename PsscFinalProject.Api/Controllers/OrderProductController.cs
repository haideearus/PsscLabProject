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

namespace PsscFinalProject.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientOrderController : ControllerBase
    {

        private readonly ILogger<ClientOrderController> logger;
        private readonly PublishOrderWorkflow publishOrderWorkflow;
        private readonly PublishBillingWorkflow publishBillingWorkflow;
        private readonly IOrderRepository orderRepository; // Add this line

        // Modify the constructor to accept IOrderRepository
        public ClientOrderController(
            ILogger<ClientOrderController> logger,
            PublishOrderWorkflow publishOrderWorkflow,
            PublishBillingWorkflow publishBillingWorkflow,
            IOrderRepository orderRepository) // Add this line
        {
            this.logger = logger;
            this.publishOrderWorkflow = publishOrderWorkflow;
            this.publishBillingWorkflow = publishBillingWorkflow;
            this.orderRepository = orderRepository; // Assign the injected repository
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
                            productsList: new List<CalculatedProduct>(), // Populate with relevant details
                            totalAmount: success.ProductPrice,
                            csv: success.Csv,
                            orderDate: success.PublishDate,
                            clientEmail: success.ClientEmail
                        ));

                        return Ok(new
                        {
                            Message = "Order placed successfully.",
                            BillingStatus = billingResult is PublishedOrderBilling ? "Billing succeeded." : "Billing failed."
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




        private async Task<bool> TriggerBilling(OrderPublishSucceededEvent succeededEvent)
        {
            try
            {
                // Assuming the client's email is already in the succeededEvent
                var bills = new[]
                {
            new InputBills
            {
                BillAddress = "Default Address", // You can replace this with a dynamic value if needed
                BillNumber = "GeneratedBillNumber", // Replace with actual logic to generate a bill number
                ClientEmail = succeededEvent.ClientEmail
            }
        };

                using (var httpClient = new HttpClient())
                {
                    var requestContent = new StringContent(
                        JsonConvert.SerializeObject(bills),
                        Encoding.UTF8,
                        "application/json"
                    );

                    // Adjust the URL to point to the actual billing service endpoint
                    var response = await httpClient.PostAsync("https://localhost:7195/Bill", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        logger.LogError($"Failed to trigger billing: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while triggering billing.");
                return false;
            }
        }

    }
}
