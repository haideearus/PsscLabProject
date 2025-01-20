using Microsoft.Extensions.Logging;
using PsscFinalProject.Data.Repositories;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Operations;
using PsscFinalProject.Domain.Repositories;
using static PsscFinalProject.Domain.Models.OrderPublishEvent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using static PsscFinalProject.Domain.Models.OrderProducts;

public class PublishOrderWorkflow
{
    private readonly IClientRepository clientRepository;
    private readonly IOrderRepository orderRepository;
    private readonly IProductRepository productRepository;
    private readonly IOrderItemRepository orderItemRepository;
    private readonly ILogger<PublishOrderWorkflow> logger;
    private readonly CalculateOrderOperation calculateOrderOperation;
    private readonly OrderStateOperation orderStateOperation;

    public PublishOrderWorkflow(
        IClientRepository clientRepository,
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IOrderItemRepository orderItemRepository,
        ILogger<PublishOrderWorkflow> logger,
        CalculateOrderOperation calculateOrderOperation,
        OrderStateOperation orderStateOperation)
    {
        this.clientRepository = clientRepository;
        this.orderRepository = orderRepository;
        this.productRepository = productRepository;
        this.orderItemRepository = orderItemRepository;
        this.logger = logger;
        this.calculateOrderOperation = calculateOrderOperation;
        this.orderStateOperation = orderStateOperation;
    }

    public async Task<IOrderPublishEvent> ExecuteAsync(PublishOrderCommand command)
    {
        try
        {
            // Step 1: Validate client association
            var clientEmail = command.InputProducts.First().ClientEmail;
            var existingClients = await clientRepository.GetExistingClientsAsync(new List<string> { clientEmail });

            if (!existingClients.Any())
            {
                logger.LogWarning("Client not found: {ClientEmail}", clientEmail);
                return new OrderPublishFailedEvent(new List<string> { $"Client with email {clientEmail} does not exist." });
            }

            // Step 2: Validate product codes in the order
            var productCodes = command.InputProducts.Select(p => p.ProductCode).Distinct();
            var existingProducts = await productRepository.GetProductsByCodesAsync(productCodes);

            var missingProductCodes = productCodes.Except(existingProducts.Select(p => p.code.Value)).ToList();
            if (missingProductCodes.Any())
            {
                logger.LogWarning("Products not found: {ProductCodes}", string.Join(", ", missingProductCodes));
                return new OrderPublishFailedEvent(new List<string> { $"Products not found: {string.Join(", ", missingProductCodes)}" });
            }

            // Step 3: Validate the order
            var validateOrderOperation = new ValidateOrderOperation(productRepository);
            var unvalidatedOrder = new UnvalidatedOrderProducts(command.InputProducts);
            var validatedOrder = validateOrderOperation.Transform(unvalidatedOrder);

            if (validatedOrder is InvalidOrderProducts invalidOrder)
            {
                logger.LogWarning("Order validation failed: {Reasons}", string.Join(", ", invalidOrder.Reasons));
                return new OrderPublishFailedEvent(invalidOrder.Reasons.ToList());
            }

            var calculateOrderOperation = new CalculateOrderOperation(orderItemRepository);
            var calculatedOrder = calculateOrderOperation.Transform((ValidatedOrderProducts)validatedOrder);

            if (calculatedOrder is not CalculatedOrder finalOrder)
            {
                logger.LogError("Order calculation failed.");
                return new OrderPublishFailedEvent(new List<string> { "Failed to calculate the order." });
            }

            // Step 4: Save the order to the database
            var paidOrder = new PaidOrderProducts(
                finalOrder.ProductList,
                new ProductPrice(finalOrder.ProductList.Sum(p => p.totalPrice.Value)),
                GenerateCsv(finalOrder),
                DateTime.Now,
                finalOrder.ClientEmail
            );

            await orderRepository.SaveOrdersAsync(paidOrder);

            // Step 5: Publish the success event
            logger.LogInformation("Order published successfully: {ClientEmail}", finalOrder.ClientEmail.Value);
            return new OrderPublishSucceededEvent(
                finalOrder.ClientEmail,
                GenerateCsv(finalOrder),
                new ProductPrice(finalOrder.ProductList.Sum(p => p.totalPrice.Value)),
                DateTime.Now
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
            "ClientEmail,ProductCode,Price,Quantity,TotalPrice"
        };

        csvLines.AddRange(order.ProductList.Select(product =>
            $"{product.clientEmail.Value},{product.productCode.Value},{product.productPrice.Value:F2},{product.productQuantity.Value},{product.totalPrice.Value:F2}"));

        return string.Join(Environment.NewLine, csvLines);
    }
}
