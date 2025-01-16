using Microsoft.AspNetCore.Mvc;
using PsscFinalProject.Api.Models;
using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using static PsscFinalProject.Domain.Models.AddOrderItemCommand;

namespace PsscFinalProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemService;
        private readonly IProductRepository _productService;
        private readonly ILogger<OrderItemsController> _logger;

        public OrderItemsController(IOrderItemRepository orderItemService, IProductRepository productService, ILogger<OrderItemsController> logger)
        {
            _orderItemService = orderItemService;
            _productService = productService;
            _logger = logger;
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddItemsToOrder([FromBody] AddOrderItemsCommand command)
        {
            try
            {
                if (command == null || command.OrderId <= 0 || command.ProductCodes == null || !command.ProductCodes.Any())
                {
                    _logger.LogWarning("Invalid data for adding items to order.");
                    return BadRequest("Invalid data.");
                }

                // Fetch products from the database
                var products = await _productService.GetProductsByCodesAsync(command.ProductCodes);

                if (products == null || !products.Any())
                {
                    return NotFound("No valid products found for the provided codes.");
                }

                // Map ValidatedProduct to OrderItem
                var orderItems = products.Select(p => new OrderItem(
                    command.OrderId, // Use the provided OrderId
                    p.ProductCode.Value, // Convert custom type to string
                    p.ProductPrice.Value, // Convert custom type to decimal
                    1 // Default quantity
                )).ToList();

                // Save order items
                await _orderItemService.AddOrderItemsAsync(orderItems);

                return Ok(new { Message = "Products added to the order successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding items to the order.");
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }



    }

}
