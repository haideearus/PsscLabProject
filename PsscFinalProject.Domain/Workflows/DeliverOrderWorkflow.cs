using Microsoft.Extensions.Logging;
using PsscFinalProject.Domain.Operations;
using PsscFinalProject.Domain.Repositories;
using static PsscFinalProject.Domain.Models.OrderDelivery;
using static PsscFinalProject.Domain.Models.OrderProducts;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

public class PublishDeliveryWorkflow
{
    private readonly IOrderRepository orderRepository;
    private readonly IDeliveryRepository deliveryRepository;
    private readonly ILogger<PublishDeliveryWorkflow> logger;

    public PublishDeliveryWorkflow(
        IOrderRepository orderRepository,
        IDeliveryRepository deliveryRepository,
        ILogger<PublishDeliveryWorkflow> logger)
    {
        this.orderRepository = orderRepository;
        this.deliveryRepository = deliveryRepository;
        this.logger = logger;
    }

    public async Task<IOrderDelivery> ExecuteAsync(UnvalidatedOrderDelivery unvalidatedDelivery, PaidOrderProducts paidOrder, int orderId)
    {
        try
        {
            // Step 1: Log details from PaidOrderProducts
            logger.LogInformation("Paid order details: {ClientEmail}, {OrderDate}, {OrderId}",
                paidOrder.ClientEmail.Value, paidOrder.OrderDate, orderId);

            // Step 2: Validate the deliveries
            var validateDeliveryOperation = new ValidateDeliveryOperation();
            var validatedDelivery = validateDeliveryOperation.Transform(unvalidatedDelivery, null);

            if (validatedDelivery is InvalidOrderDelivery invalidDelivery)
            {
                logger.LogWarning("Delivery validation failed: {Reasons}", string.Join(", ", invalidDelivery.Reasons));
                return invalidDelivery;
            }

            // Step 3: Calculate delivery details
            var calculateDeliveryOperation = new CalculateDeliveryOperation();
            var calculatedDelivery = calculateDeliveryOperation.Transform(validatedDelivery, null);

            if (calculatedDelivery is not CalculatedOrderDelivery calculated)
            {
                logger.LogError("Delivery calculation failed.");
                return new FailedOrderDelivery(unvalidatedDelivery.DeliveryList, new Exception("Failed to calculate deliveries."));
            }

            // Step 4: Transform to PublishedOrderDelivery
            logger.LogInformation("Transforming to PublishedOrderDelivery.");
            var csvContent = GenerateCsv(calculated, paidOrder);
            var publishedDelivery = new PublishedOrderDelivery(calculated.DeliveryList, csvContent, DateTime.UtcNow);

            // Step 5: Save PublishedOrderDelivery to the database
            logger.LogInformation("Saving PublishedOrderDelivery to the database.");
            await deliveryRepository.SaveDeliveries(publishedDelivery);

            // Step 6: Return the published delivery
            logger.LogInformation("Delivery workflow completed successfully.");
            return publishedDelivery;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during the delivery workflow.");
            return new FailedOrderDelivery(unvalidatedDelivery.DeliveryList, ex);
        }
    }

    private string GenerateCsv(CalculatedOrderDelivery calculatedDelivery, PaidOrderProducts paidOrder)
    {
        var csvLines = new List<string> { "TrackingNumber,Courier,ClientEmail,TotalAmount" };
        csvLines.AddRange(calculatedDelivery.DeliveryList.Select(delivery =>
            $"{delivery.TrackingNumber.Value},{delivery.Courier.Value},{paidOrder.ClientEmail.Value},{paidOrder.TotalAmount.Value:F2}"));
        return string.Join(Environment.NewLine, csvLines);
    }
}
