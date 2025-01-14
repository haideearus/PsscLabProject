using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.Order;

namespace PsscFinalProject.Domain.Operations
{
    internal sealed class PublishOrderOperation : OrderOperation
    {
        protected override IOrder OnCalculated(CalculatedOrder calculatedOrder)
        {
            StringBuilder csv = new StringBuilder();
            calculatedOrder.ProductList.Aggregate(csv, (export, product) =>
                export.AppendLine(GenerateCsvLine(product)));

            // Include all required parameters for PaidOrder
            PaidOrder paidOrder = new(
                calculatedOrder.ClientEmail,
                calculatedOrder.ProductList,
                csv.ToString(),
                calculatedOrder.OrderDate,           // Order date
                calculatedOrder.PaymentMethod,       // Payment method
                calculatedOrder.TotalAmount,         // Total amount
                calculatedOrder.ShippingAddress,     // Shipping address
                calculatedOrder.State,               // State
                null                                 // Optional OrderId, null at this stage
            );

            return paidOrder;
        }

        private static string GenerateCsvLine(CalculatedProduct product) =>
            $"{product.ProductCode.Value}, {product.ProductPrice.Value:F2}, {product.ProductQuantity.Value}, {product.TotalPrice:F2}";
    }
}
