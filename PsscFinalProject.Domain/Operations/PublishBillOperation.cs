using PsscFinalProject.Domain.Models;
using System;
using System.Linq;
using static PsscFinalProject.Domain.Models.OrderBilling;

namespace PsscFinalProject.Domain.Operations
{
    internal sealed class PublishOrderBillingOperation : OrderBillingOperation
    {
        protected override IOrderBilling OnCalculated(CalculatedOrderBilling calculatedOrderBilling)
        {
            var csv = string.Join("\n", calculatedOrderBilling.BillList.Select(bill =>
                $"{bill.BillNumber.Value},{bill.ShippingAddress.Value},{bill.ProductPrice.Value}"));

            return new PublishedOrderBilling(
                billsList: calculatedOrderBilling.BillList,
                csv: csv,
                publishedDate: DateTime.Now
            );
        }
    }
}
