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
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;
        private readonly IAddressRepository addressRepository;

        public ClientOrderController(
        ILogger<ClientOrderController> logger,
        PublishOrderWorkflow publishOrderWorkflow,
        PublishBillingWorkflow publishBillingWorkflow,
        IOrderRepository orderRepository,
        PublishDeliveryWorkflow publishDeliveryWorkflow,
        IProductRepository productRepository,
        IAddressRepository addressRepository)
        {
            this.logger = logger;
            this.publishOrderWorkflow = publishOrderWorkflow;
            this.publishBillingWorkflow = publishBillingWorkflow;
            this.orderRepository = orderRepository;
            this.publishDeliveryWorkflow = publishDeliveryWorkflow;
            this.productRepository = productRepository;
            this.addressRepository = addressRepository; 
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

        [HttpGet("getOrdersByEmail/{email}")]
        public async Task<IActionResult> GetOrdersByEmail(string email)
        {
            try
            {
                var orders = await orderRepository.GetOrdersByEmailAsync(email);

                if (orders == null || !orders.Any())
                {
                    return NotFound(new { Message = $"No orders found for email {email}." });
                }

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
                logger.LogError(ex, "An error occurred while fetching orders by email.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching orders by email.");
            }
        }

        [HttpPost("placeOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] IEnumerable<UnvalidatedOrder> unvalidatedOrders)
        {
            try
            {
                // Execute the order placement workflow
                var command = new PublishOrderCommand(unvalidatedOrders.ToList().AsReadOnly());
                var result = await publishOrderWorkflow.ExecuteAsync(command);

                switch (result)
                {
                    case OrderPublishSucceededEvent success:
                        // Retrieve the client's shipping address
                        var shippingAddress = await FetchShippingAddress(success.ClientEmail);

                        if (string.IsNullOrWhiteSpace(shippingAddress))
                        {
                            return BadRequest(new { Message = $"No shipping address found for client {success.ClientEmail.Value}." });
                        }

                        var existingOrders = await orderRepository.GetExistingOrdersAsync();
                        var existingOrder = existingOrders.FirstOrDefault(o => o.clientEmail.Value == success.ClientEmail.Value);

                        if (existingOrder == null)
                        {
                            return NotFound($"Order for client {success.ClientEmail} not found.");
                        }

                        var billNumber = BillNumber.Generate();

                        var unvalidatedBilling = new UnvalidatedOrderBilling(new List<UnvalidatedBill>
                {
                    new UnvalidatedBill(billNumber.Value, shippingAddress, success.ProductPrice.Value)
                }.AsReadOnly());

                        // Trigger the billing workflow
                        var billingResult = await publishBillingWorkflow.ExecuteAsync(unvalidatedBilling, new PaidOrderProducts(
                            productsList: new List<CalculatedProduct> { existingOrder },
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

                        // Update stock for each product in the order
                        foreach (var order in unvalidatedOrders)
                        {
                            try
                            {
                                var productCode = new ProductCode(order.ProductCode);
                                var quantity = new ProductQuantity(order.Quantity);

                                await productRepository.UpdateStockAsync(productCode, quantity);
                            }
                            catch (InvalidOperationException ex)
                            {
                                logger.LogError(ex, $"Stock update failed for product {order.ProductCode}. Insufficient stock.");
                                return BadRequest(new
                                {
                                    Message = $"Order failed due to insufficient stock for product {order.ProductCode}.",
                                    Error = ex.Message
                                });
                            }
                        }

                        var unvalidatedDelivery = new UnvalidatedOrderDelivery(new List<UnvalidatedDelivery>
                        {
                            new UnvalidatedDelivery(
                                TrackingNumber.Generate().Value,
                                Courier.GetCourier()//"Sameday" // Example courier, this can be dynamic or user-provided
                            )
                        }.AsReadOnly());

                        // Trigger the delivery workflow
                        var deliveryResult = await publishDeliveryWorkflow.ExecuteAsync(unvalidatedDelivery, new PaidOrderProducts(
                            productsList: new List<CalculatedProduct> { existingOrder },
                            totalAmount: success.ProductPrice,
                            csv: success.Csv,
                            orderDate: success.PublishDate,
                            clientEmail: success.ClientEmail
                        ), existingOrder.OrderId);

                        return Ok(new
                        {
                            Message = "Order placed successfully.",
                            BillingStatus = "Billing succeeded.",
                            DeliveryStatus = deliveryResult is PublishedOrderDelivery ? "Delivery succeeded." : "Delivery failed.",
                            StockStatus = "Stock updated successfully.",
                            ShippingAddress = shippingAddress
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

        // Helper method to fetch shipping address
        private async Task<string> FetchShippingAddress(ClientEmail clientEmail)
        {
            var address = await addressRepository.GetAddressesByClientEmailAsync(clientEmail);

            // Return the first address or handle the case where no address exists
            return address.FirstOrDefault()?.Value ?? throw new InvalidOperationException($"No address found for client {clientEmail.Value}");
        }
    }
}