using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.OrderProducts;

namespace PsscFinalProject.Domain.Operations
{
    internal sealed class PublishOrderOperation : OrderOperation
    {
        protected override IOrderProducts OnCalculated(CalculatedOrder calculatedOrder)
        {
            var csv = string.Join("\n", calculatedOrder.ProductList.Select(product =>
                $"{product.clientEmail.Value},{product.productCode.Value},{product.productPrice.Value},{product.productQuantity.Value},{product.totalPrice.Value}"));

            var totalAmount = new ProductPrice(calculatedOrder.ProductList.Sum(product => product.totalPrice.Value));

            return new PaidOrderProducts(
                productsList: calculatedOrder.ProductList,
                totalAmount: totalAmount,
                csv: csv,
                orderDate: DateTime.Now,
                clientEmail: calculatedOrder.ClientEmail
            );
        }
    }
}
