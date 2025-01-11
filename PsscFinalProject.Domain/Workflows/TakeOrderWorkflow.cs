//using Microsoft.Extensions.Logging;
//using PsscFinalProject.Data.Repositories;
//using PsscFinalProject.Domain.Models;
//using PsscFinalProject.Domain.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//public class TakeOrderWorkflow
//{
//    private readonly IOrderRepository orderRepository;
//    private readonly IClientRepository clientRepository;
//    private readonly ILogger<TakeOrderWorkflow> logger;

//    public TakeOrderWorkflow(
//        IOrderRepository orderRepository,
//        IClientRepository clientRepository,
//        ILogger<TakeOrderWorkflow> logger)
//    {
//        this.orderRepository = orderRepository;
//        this.clientRepository = clientRepository;
//        this.logger = logger;
//    }

//    public async Task<IOrderProcessedEvent> ExecuteAsync(TakeOrderCommand command)
//    {
//        try
//        {
//            // 1. Retrieve or create a new empty shopping cart for the client (this cart is specific to this order)
//            var shoppingCart = await CreateNewShoppingCartAsync(command.ClientId, command.ClientEmail);

//            // 2. Add products to the shopping cart
//            shoppingCart = shoppingCart.AddProduct(command.ProductIds);

//            // 3. Compute the total amount and validate the cart
//            decimal totalAmount = await ComputeTotalAmountAsync(command.ProductIds); // Simulate total price calculation
//            shoppingCart = shoppingCart.ComputeTotal(totalAmount);

//            // 4. Pay for the cart and transition it to the "Paid" state
//            shoppingCart = shoppingCart.Pay();

//            // 5. Create an order from the paid cart (this is where the order is placed)
//            var order = await shoppingCart.PlaceOrderAsync(command.ShippingAddress);

//            // 6. Save the order to the database
//            //await orderRepository.SaveOrderAsync(order);

//            // 7. Return a success event indicating the order was processed successfully
//            logger.LogInformation($"Order placed successfully for Client ID {shoppingCart.ClientId}.");
//            return new OrderProcessedEvent(order.OrderId, "Order has been successfully processed.");
//        }
//        catch (Exception ex)
//        {
//            logger.LogError(ex, "An error occurred while processing the order.");
//            return new OrderProcessingFailedEvent("An unexpected error occurred.");
//        }
//    }

//    // Helper method to create a new empty shopping cart for the client (this cart is fresh and specific to this order)
//    private async Task<ShoppingCart> CreateNewShoppingCartAsync(int clientId, string clientEmail)
//    {
//        // Retrieve the client to verify existence
//        var client = await clientRepository.GetClientByIdAsync(clientId);

//        if (client == null)
//        {
//            throw new InvalidOperationException("Client not found.");
//        }

//        // Create a new empty shopping cart (new order, no products yet)
//        var shoppingCart = new ShoppingCart(clientId, clientEmail, clientRepository);

//        logger.LogInformation($"Created a new empty shopping cart for Client ID {clientId}.");

//        return shoppingCart;
//    }

//    // Simulate total price calculation (replace with actual product pricing logic)
//    private Task<decimal> ComputeTotalAmountAsync(List<string> productIds)
//    {
//        // For demonstration, let's assume each product costs $10. Replace this with actual pricing logic.
//        decimal totalAmount = productIds.Count * 10.00m; // Example: each product costs $10
//        return Task.FromResult(totalAmount);
//    }
//}