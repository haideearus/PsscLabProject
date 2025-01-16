using NUnit.Framework;
using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PsscFinalProject.Domain.Operations;
using System.Text;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.Order;
using Assert = NUnit.Framework.Assert;
using Microsoft.Extensions.Logging;
using Moq;

namespace PsscFinalProject.Tests.Tests
{
    internal class Class1
    {
        [Test]
        public void TestCalculateOrderOperation()
        {
            //var mockLogger = new Mock<ILogger<CalculateOrderOperation>>();
            var operation = new CalculateOrderOperation();

            var productList = new List<ValidatedProduct>
    {
        new ValidatedProduct(
            ProductName.Create("Milk"),
            ProductCode.Create("PRD001"),
            ProductPrice.Create(1.49M),
            ProductQuantityType.Create("Unit"),
            ProductQuantity.Create(6)
        ),
        new ValidatedProduct(
            ProductName.Create("Bread"),
            ProductCode.Create("PRD002"),
            ProductPrice.Create(2.00M),
            ProductQuantityType.Create("Unit"),
            ProductQuantity.Create(3)
        )
    };

            var validatedOrder = new ValidatedOrder(new ClientEmail("eu.eu@gmail.com"), productList);

            var result = operation.Transform(validatedOrder);

            Assert.That(result, Is.InstanceOf<CalculatedOrder>());
            Assert.That(((CalculatedOrder)result).TotalAmount, Is.EqualTo(14.94M));
        }

        [Test]
        public void OnValidated_ShouldCreateCalculatedOrderWithCorrectProperties()
        {
            // Arrange
            var validatedOrder = new ValidatedOrder(
                new ClientEmail("test@example.com"),
                new List<ValidatedProduct>
                {
            new ValidatedProduct(new ProductName("ProductA"), new ProductCode("PRD001"), new ProductPrice(10m), new ProductQuantityType("Unit"), new ProductQuantity(2)),
            new ValidatedProduct(new ProductName("ProductB"), new ProductCode("PRD002"), new ProductPrice(20m), new ProductQuantityType("Unit"), new ProductQuantity(1))
                }
            );

            var operation = new CalculateOrderOperation();

            // Act
            var result = operation.Transform(validatedOrder);

            // Assert
            Assert.That(result, Is.InstanceOf<CalculatedOrder>(), "The result should be a CalculatedOrder.");
            var calculatedOrder = (CalculatedOrder)result;

            Assert.That(calculatedOrder.TotalAmount, Is.EqualTo(40m), "Total amount should match the expected value.");
            Assert.That(calculatedOrder.ShippingAddress, Is.EqualTo("Default Address"), "Shipping address should be the default value.");
            Assert.That(calculatedOrder.PaymentMethod, Is.EqualTo(1), "Payment method should be the default value.");
            Assert.That(calculatedOrder.State, Is.EqualTo(1), "Order state should be 'Placed'.");
            Assert.That(calculatedOrder.ClientEmail.Value, Is.EqualTo("test@example.com"), "Client email should match.");

        }

    }
}
