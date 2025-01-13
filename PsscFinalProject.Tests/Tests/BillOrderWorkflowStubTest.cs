/*using NUnit.Framework;
using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Tests.Stubs;
using Assert = NUnit.Framework.Assert;

namespace PsscFinalProject.Tests.Tests
{
    [TestFixture]
    public class BillOrderWorkflowStubTests
    {
        [Test]
        public async Task GetBillsAsync_ReturnsBills()
        {
            // Arrange
            var context = new PsscDbContext(); 
            var workflow = new BillOrderWorkflowStub(context);

            // Act
            var bills = await workflow.GetBillsAsync();

            // Assert
            Assert.That(bills, Is.Not.Null);
            Assert.That(bills.Count, Is.EqualTo(2));
            Assert.That(bills[0].OrderId, Is.EqualTo(123));
            Assert.That(bills[0].TotalAmount, Is.EqualTo(200));
            Assert.That(bills[0].Order?.Client?.Email, Is.EqualTo("haidee.rus@example.com"));
        }

        [Test]
        public async Task GetBillByIdAsync_BillExists_ReturnsBill()
        {
            // Arrange
            var context = new PsscDbContext();
            var workflow = new BillOrderWorkflowStub(context);

            var client = new Client(1, "haidee.rus@example.com");

            var order = new Order(
                client,
                DateTime.Now,
                PaymentMethod.CreditCard,
                200,
                "Address 123"
            );

            var bill = new Bill
            {
                BillId = 1,
                OrderId = 123,
                TotalAmount = 200,
                BillingDate = DateTime.Now,
                Order = order
            };

            var bills = new List<Bill> { bill };

            // Act
            await workflow.SaveBillsAsync(bills);

            // Assert
            Assert.That(bills[0].OrderId, Is.EqualTo(123));
            Assert.That(bills[0].TotalAmount, Is.EqualTo(200));
            Assert.That(bills[0].Order?.Client?.Email, Is.EqualTo("haidee.rus@example.com"));
        }

        [Test]
        public void GetBillByIdAsync_BillNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var context = new PsscDbContext();
            var workflow = new BillOrderWorkflowStub(context);

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => workflow.GetBillByIdAsync(999));
        }

        [Test]
        public async Task SaveBillsAsync_SavesBillsSuccessfully_WithExistingClient()
        {
            // Arrange
            var context = new PsscDbContext();
            var workflow = new BillOrderWorkflowStub(context);

            var existingClient = new Client(1, "haidee.rus@example.com");

            var order = new Order(
                existingClient,
                DateTime.Now,
                PaymentMethod.PayPal,
                300,
                "Address 789"
            );

            var bills = new List<Bill>
            {
                new Bill
                {
                    BillId = 3,
                    OrderId = 125,
                    TotalAmount = 300,
                    BillingDate = DateTime.Now,
                    Order = order
                }
            };

            // Act
            await workflow.SaveBillsAsync(bills);

            // Assert - check db
        }
    }
}*/