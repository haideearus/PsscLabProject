using Microsoft.Extensions.Logging;
using PsscFinalProject.Domain.Operations;
using PsscFinalProject.Domain.Repositories;
using static PsscFinalProject.Domain.Models.OrderBilling;
using static PsscFinalProject.Domain.Models.OrderProducts;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;

public class PublishBillingWorkflow
{
    private readonly IOrderRepository orderRepository;
    private readonly IBillRepository billRepository;
    private readonly ILogger<PublishBillingWorkflow> logger;

    public PublishBillingWorkflow(
        IOrderRepository orderRepository,
        IBillRepository billRepository,
        ILogger<PublishBillingWorkflow> logger)
    {
        this.orderRepository = orderRepository;
        this.billRepository = billRepository;
        this.logger = logger;
    }

    public async Task<IOrderBilling> ExecuteAsync(UnvalidatedOrderBilling unvalidatedBilling, PaidOrderProducts paidOrder)
    {
        try
        {
            // Step 1: Log details from PaidOrderProducts
            logger.LogInformation("Using paid order details: {ClientEmail}, {OrderDate}",
                paidOrder.ClientEmail.Value, paidOrder.OrderDate);

            // Step 2: Validate the bills
            var validateBillOperation = new ValidateBillOperation();
            var validatedBilling = validateBillOperation.Transform(unvalidatedBilling, null);

            if (validatedBilling is InvalidOrderBilling invalidBilling)
            {
                logger.LogWarning("Billing validation failed: {Reasons}", string.Join(", ", invalidBilling.Reasons));
                return invalidBilling;
            }

            // Step 3: Calculate totals for the bills
            var calculateBillOperation = new CalculateBillOperation();
            var calculatedBilling = calculateBillOperation.Transform(validatedBilling, null);

            if (calculatedBilling is not CalculatedOrderBilling calculated)
            {
                logger.LogError("Billing calculation failed.");
                return new FailedOrderBilling(unvalidatedBilling.BillList, new Exception("Failed to calculate bills."));
            }

            // Step 4: Transform to PublishedOrderBilling
            logger.LogInformation("Transforming to PublishedOrderBilling.");
            var csvContent = GenerateCsv(calculated);
            var publishedBilling = new PublishedOrderBilling(calculated.BillList, csvContent, DateTime.UtcNow);

            // Step 5: Save PublishedOrderBilling to the database
            logger.LogInformation("Saving PublishedOrderBilling to the database.");
            await billRepository.SaveBills(publishedBilling);

            // Step 6: Return the published billing
            logger.LogInformation("Billing workflow completed successfully.");
            return publishedBilling;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during the billing workflow.");
            return new FailedOrderBilling(unvalidatedBilling.BillList, ex);
        }
    }

    private string GenerateCsv(CalculatedOrderBilling calculatedBilling)
    {
        var csvLines = new List<string> { "BillNumber,ShippingAddress,TotalAmount" };
        csvLines.AddRange(calculatedBilling.BillList.Select(bill =>
            $"{bill.BillNumber.Value},{bill.ShippingAddress.Value},{bill.ProductPrice.Value:F2}"));
        return string.Join(Environment.NewLine, csvLines);
    }
}
