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

            PaidOrder paidOrder = new(calculatedOrder.ProductList, csv.ToString(), DateTime.Now);
            return paidOrder;
        }

        private static string GenerateCsvLine(CalculatedProduct product) =>
            $"{product.ProductId}, {product.Name}, {product.Price}, {product.Quantity}, {product.TotalPrice}";
    }
}
