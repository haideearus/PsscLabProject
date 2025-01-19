using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Operations;
using System;
using System.Linq;
using static PsscFinalProject.Domain.Models.OrderBilling;

internal class CalculateBillOperation : OrderBillingOperation
{
    internal CalculateBillOperation() { }

    public override IOrderBilling Transform(IOrderBilling? billing, object? state)
    {
        if (billing == null)
        {
            throw new ArgumentNullException(nameof(billing), "Billing cannot be null");
        }

        return billing switch
        {
            ValidatedOrderBilling validatedBilling => OnValidated(validatedBilling),
            _ => throw new InvalidOperationException("Unexpected billing type")
        };
    }

    protected override IOrderBilling OnValidated(ValidatedOrderBilling validatedBilling)
    {
        if (validatedBilling == null)
        {
            throw new ArgumentNullException(nameof(validatedBilling), "Validated billing cannot be null");
        }

        // Use precomputed TotalAmount and ShippingAddress from the order workflow
        var calculatedBills = validatedBilling.BillList
            .Select(bill =>
            {
                return new CalculatedBillNumber(
                    ShippingAddress:(bill.ShippingAddress), // Precomputed ShippingAddress
                    BillNumber: (bill.BillNumber), // Format BillNumber
                    ProductPrice: (bill.TotalAmount) // Use TotalAmount from order workflow
                );
            }).ToList();

        // Create and return CalculatedOrderBilling
        return new CalculatedOrderBilling(calculatedBills.AsReadOnly());
    }
}
