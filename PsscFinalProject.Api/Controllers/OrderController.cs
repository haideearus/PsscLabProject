using Microsoft.AspNetCore.Mvc;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using PsscFinalProject.Data.Models;
using static PsscFinalProject.Domain.Models.OrderPublishEvent;
using static PsscFinalProject.Domain.Models.OrderProducts;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly PublishOrderWorkflow _workflow;
    private readonly ILogger<OrdersController> _logger;
    private readonly IOrderRepository _orderRepository; 

    public OrdersController(PublishOrderWorkflow workflow, ILogger<OrdersController> logger, IOrderRepository orderRepository)
    {
        _workflow = workflow;
        _logger = logger;
        _orderRepository = orderRepository; // Initialize repository
    }

    /// <summary>
    /// Retrieves the current state of an order.
    /// </summary>
    /// <param name="orderId">The ID of the order.</param>
    /// <returns>The current state of the order.</returns>
    [HttpGet("{orderId}/state")]
    public async Task<IActionResult> GetOrderState(int orderId)
    {
        try
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                return NotFound(new { message = $"Order with ID {orderId} not found." });
            }

            var orderState = GetOrderState(order);

            return Ok(new { orderId = orderId, state = orderState });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the order state.");
            return StatusCode(500, new { message = "An internal error occurred.", details = ex.Message });
        }
    }

    private string GetOrderState(PaidOrderProducts order)
    {
        var orderDate = order.OrderDate;

        if (orderDate.AddMinutes(5) > DateTime.Now)
        {
            return "Pending"; // Order is in 'Pending' state if created within the last 5 minutes
        }
        else if (orderDate.AddDays(1) > DateTime.Now)
        {
            return "Billed"; // Order is in 'Billed' state if created within the last day
        }
        else
        {
            return "Delivered"; // Order is in 'Delivered' state if created more than a day ago
        }
    }
}