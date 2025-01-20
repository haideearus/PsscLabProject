using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Operations;
using PsscFinalProject.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.OrderProducts;

public class CalculateOrderOperation : OrderOperation
{
    private readonly IOrderItemRepository _orderItemRepository;

    public CalculateOrderOperation(IOrderItemRepository orderItemRepository)
    {
        _orderItemRepository = orderItemRepository;
    }

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

        var calculatedProducts = _orderItemRepository.GetProductsAsync().Result; 

        var mappedProducts = validatedOrder.ProductList.Select(validatedProduct =>
        {
            var product = calculatedProducts.FirstOrDefault(p => p.productCode == validatedProduct.ProductCode);

            if (product == null)
            {
                throw new ArgumentException($"Product with code {validatedProduct.ProductCode.Value} not found in database");
            }

            return new CalculatedProduct(
                clientEmail: validatedProduct.ClientEmail,
                productCode: validatedProduct.ProductCode,
                productPrice: product.productPrice,
                productQuantityType: product.productQuantityType,
                productQuantity: validatedProduct.ProductQuantity,
                totalPrice: new ProductPrice(product.productPrice.Value * validatedProduct.ProductQuantity.Value)
            );
        }).ToList();

        return new CalculatedOrder(
            productList: mappedProducts.AsReadOnly(),
            clientEmail: validatedOrder.ProductList.First().ClientEmail
        );
    }
}
