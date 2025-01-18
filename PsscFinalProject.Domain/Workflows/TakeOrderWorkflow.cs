using Microsoft.Extensions.Logging;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Operations;
using static PsscFinalProject.Domain.Models.Order;
using static PsscFinalProject.Domain.Models.OrderPublishEvent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using PsscFinalProject.Data.Interface;

public class PublishOrderWorkflow
{
    private readonly IClientRepository clientRepository;
    private readonly IOrderRepository orderRepository;
    private readonly IProductRepository productRepository;
    private readonly IOrderItemRepository orderItemRepository;
    private readonly ILogger<PublishOrderWorkflow> logger;

    public PublishOrderWorkflow(
        IClientRepository clientRepository,
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IOrderItemRepository orderItemRepository,
        ILogger<PublishOrderWorkflow> logger)
    {
        this.clientRepository = clientRepository;
        this.orderRepository = orderRepository;
        this.productRepository = productRepository;
        this.orderItemRepository = orderItemRepository;
        this.logger = logger;
    }

    public async Task<IOrderPublishEvent> ExecuteAsync(PublishOrderCommand command)
    {
        try
        {
            // Step 1: Validate client association
            var clientEmail = command.ClientEmail;
            var clientExists = await clientRepository.ClientExistsAsync(clientEmail);

            if (!clientExists)
            {
                logger.LogWarning("Client not found: {ClientEmail}", clientEmail);
                return new OrderPublishFailedEvent(new List<string> { $"Client with email {clientEmail} does not exist." });
            }

            // Step 2: Validate product codes in the order
            var productCodes = command.ProductList.Select(p => p.ProductCode).Distinct();
            var existingProducts = await productRepository.GetExistingProductsAsync(productCodes);

            var missingProductCodes = productCodes.Except(existingProducts.Select(p => p.Value)).ToList();
            if (missingProductCodes.Any())
            {
                logger.LogWarning("Products not found: {ProductCodes}", string.Join(", ", missingProductCodes));
                return new OrderPublishFailedEvent(new List<string> { $"Products not found: {string.Join(", ", missingProductCodes)}" });
            }

            // Step 3: Validate the order
            var validateOrderOperation = new ValidateOrderOperation(productRepository);
            var unvalidatedOrder = new UnvalidatedOrder(clientEmail, command.ProductList);
            var validatedOrder = validateOrderOperation.Transform(unvalidatedOrder);

            if (validatedOrder is InvalidOrder invalidOrder)
            {
                logger.LogWarning("Order validation failed: {Reasons}", string.Join(", ", invalidOrder.Reasons));
                return new OrderPublishFailedEvent(invalidOrder.Reasons.ToList());
            }

            // Step 4: Calculate total price and prepare the order
            var calculateOrderOperation = new CalculateOrderOperation();
            var calculatedOrder = calculateOrderOperation.Transform((ValidatedOrder)validatedOrder);

            if (calculatedOrder is not CalculatedOrder finalOrder)
            {
                logger.LogError("Order calculation failed.");
                return new OrderPublishFailedEvent(new List<string> { "Failed to calculate the order." });
            }

            // Step 5: Save the order to the database
            // Step 5: Save the order to the database
            var paidOrder = new PaidOrder(
                finalOrder.ClientEmail,
                finalOrder.ProductList,
                GenerateCsv(finalOrder),
                command.OrderDate,                  // Pass order date from PublishOrderCommand
                command.PaymentMethod,              // Pass payment method from PublishOrderCommand
                command.TotalAmount,                // Pass total amount from PublishOrderCommand
                command.ShippingAddress ?? string.Empty, // Pass shipping address or a default value
                command.State,                      // Pass state from PublishOrderCommand
                null                                // Optional OrderId, null at this stage
            );
            await orderRepository.SaveOrdersAsync(paidOrder);


            // Step 5a: Add order items to the database
            if (paidOrder.OrderId.HasValue)
            {
                foreach (var product in finalOrder.ProductList)
                {
                    await orderItemRepository.AddOrderItemAsync(paidOrder.OrderId.Value, product.ProductCode.Value, product.ProductPrice.Value);
                }
            }
            else
            {
                throw new InvalidOperationException("OrderId is required to save order items.");
            }

            // Step 6: Publish the success event
            logger.LogInformation("Order published successfully: {OrderId}", paidOrder.ProductList.FirstOrDefault()?.ProductDetailId ?? 0);
            return new OrderPublishSucceededEvent(
                csv: GenerateCsv(finalOrder),
                orderDetails: finalOrder.ProductList.Select(p => new CalculatedOrderDetail(
                    p.ProductCode.Value,
                    p.ProductPrice.Value,
                    p.ProductQuantity.Value,
                    p.TotalPrice
                )).ToList()
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the order workflow.");
            return new OrderPublishFailedEvent(new List<string> { "Unexpected error occurred.", ex.Message });
        }
    }

    private string GenerateCsv(CalculatedOrder order)
    {
        // Generate CSV from the order details
        var csvLines = new List<string>
        {
            "ProductName,ProductCode,Price,Quantity,TotalPrice"
        };

        csvLines.AddRange(order.ProductList.Select(product =>
            $"{product.ProductName.Value},{product.ProductCode.Value},{product.ProductPrice.Value:F2},{product.ProductQuantity.Value},{product.TotalPrice:F2}"));

        return string.Join(Environment.NewLine, csvLines);
    }
}
