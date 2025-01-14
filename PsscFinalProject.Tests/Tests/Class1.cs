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
            var mockLogger = new Mock<ILogger<CalculateOrderOperation>>();
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


    }
}
