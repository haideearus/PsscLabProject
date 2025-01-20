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

        var calculatedBills = validatedBilling.BillList
            .Select(bill =>
            {
                return new CalculatedBillNumber(
                    ShippingAddress:(bill.ShippingAddress), 
                    BillNumber: (bill.BillNumber), 
                    ProductPrice: (bill.TotalAmount) 
                );
            }).ToList();

        return new CalculatedOrderBilling(calculatedBills.AsReadOnly());
    }
}
