//using Microsoft.Extensions.Logging;
//using PsscFinalProject.Data.Repositories;
//using PsscFinalProject.Domain.Models;
//using PsscFinalProject.Domain.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//public class TakeOrderWorkflow
//{
//    private readonly IShoppingCartRepository shoppingCartRepository;
//    private readonly IOrderRepository orderRepository;
//    private readonly ClientRepository clientRepository;
//    private readonly ILogger<TakeOrderWorkflow> logger;

//    public TakeOrderWorkflow(
//        IShoppingCartRepository shoppingCartRepository,
//        IOrderRepository orderRepository,
//        ClientRepository clientRepository,
//        ILogger<TakeOrderWorkflow> logger)
//    {
//        this.shoppingCartRepository = shoppingCartRepository;
//        this.orderRepository = orderRepository;
//        this.clientRepository = clientRepository; // Injected properly
//        this.logger = logger;
//    }

//    // Main method to execute the workflow for taking an order
//    public async Task<IOrderProcessedEvent> ExecuteAsync(TakeOrderCommand command)
//    {
//        try
//        {
//            // 1. Retrieve or create a shopping cart for the client
//            var shoppingCart = await RetrieveOrCreateShoppingCartAsync(command.ClientId, command.ClientEmail);

//            // 2. Add products to the cart, transitioning it to an Unvalidated state
//            shoppingCart = shoppingCart.AddProduct(command.ProductIds);

//            // 3. Compute total, transitioning the cart to a Validated state
//            decimal totalAmount = await ComputeTotalAmountAsync(command.ProductIds); // Simulate total price calculation
//            shoppingCart = shoppingCart.ComputeTotal(totalAmount);

//            // 4. Pay the cart, transitioning it to the Paid state
//            shoppingCart = shoppingCart.Pay();

//            // 5. Create an order from the paid cart
//            var order = await shoppingCart.PlaceOrderAsync(command.ShippingAddress);

//            // 6. Save the order to the database
//            await orderRepository.SaveOrderAsync(order);

//            // 7. Save the final cart state to the repository
//            await shoppingCartRepository.SaveAsync(shoppingCart);

//            // 8. Return success event
//            logger.LogInformation($"Order placed successfully for Client ID {shoppingCart.ClientId}.");
//            return new OrderProcessedEvent(order.OrderId, "Order has been successfully processed.");
//        }
//        catch (Exception ex)
//        {
//            logger.LogError(ex, "An error occurred while processing the order.");
//            return new OrderProcessingFailedEvent("An unexpected error occurred.");
//        }
//    }

//    // Helper method to retrieve or create a shopping cart
//    private async Task<ShoppingCart> RetrieveOrCreateShoppingCartAsync(int clientId, string clientEmail)
//    {
//        var shoppingCart = await shoppingCartRepository.GetCartByClientIdAsync(clientId);
//        if (shoppingCart == null)
//        {
//            // Create a new cart if one doesn't exist
//            shoppingCart = new ShoppingCart(clientId, clientEmail, clientRepository);
//            logger.LogInformation($"New shopping cart created for Client ID {clientId}.");
//        }
//        return shoppingCart;
//    }

//    // Simulate total price calculation (replace with actual implementation)
//    private Task<decimal> ComputeTotalAmountAsync(List<string> productIds)
//    {
//        // Here you could fetch product prices and calculate the total amount
//        decimal totalAmount = productIds.Count * 10.00m; // Example: each product costs $10
//        return Task.FromResult(totalAmount);
//    }
//}
