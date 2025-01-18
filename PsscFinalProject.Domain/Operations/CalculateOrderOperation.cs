using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Operations;
using System;
using System.Linq;
using static PsscFinalProject.Domain.Models.OrderProducts;

internal class CalculateOrderOperation : OrderOperation
{
    internal CalculateOrderOperation() { }

    public override IOrderProducts Transform(IOrderProducts? order, object? state)
    {
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order), "Order cannot be null");
        }

        return order switch
        {
            ValidatedOrderProducts validatedOrder => OnValidated(validatedOrder),
            _ => throw new InvalidOperationException("Unexpected order type")
        };
    }

    protected override IOrderProducts OnValidated(ValidatedOrderProducts validatedOrder)
    {
        if (validatedOrder == null)
        {
            throw new ArgumentNullException(nameof(validatedOrder), "Validated order cannot be null");
        }

        // Calculate total price for each product
        var calculatedProducts = validatedOrder.ProductList
            .Select(product =>
            {
                var totalPrice = product.ProductQuantity.Value * 100; // Adjust price logic as needed
                return new CalculatedProduct(
                    clientEmail: product.ClientEmail,
                    productCode: product.ProductCode,
                    productPrice: new ProductPrice(100), // Replace with actual price
                    productQuantityType: new ProductQuantityType("Unit"),
                    productQuantity: product.ProductQuantity,
                    totalPrice: new ProductPrice(totalPrice)
                );
            }).ToList();

        // Create and return a CalculatedOrder
        return new CalculatedOrder(
            productList: calculatedProducts.AsReadOnly(),
            clientEmail: validatedOrder.ProductList.First().ClientEmail
        );
    }
}
