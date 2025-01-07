using PsscFinalProject.Data.Models;
using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Tests.Stubs
{
    public class BillOrderWorkflowStub : BillOrderWorkflow
    {
        public BillOrderWorkflowStub(PsscDbContext context) : base(context) { }

        public override Task<List<Bill>> GetBillsAsync()
        {
            var mockClient = new Client(1, "haidee.rus@example.com");

            var bills = new List<Bill>
        {
            new Bill
            {
                BillId = 1,
                OrderId = 123,
                TotalAmount = 200,
                BillingDate = DateTime.Now,
                Order = new Order(mockClient, DateTime.Now, PaymentMethod.CreditCard, 200, "Address 123")
            },
            new Bill
            {
                BillId = 2,
                OrderId = 124,
                TotalAmount = 150,
                BillingDate = DateTime.Now,
                Order = new Order(mockClient, DateTime.Now, PaymentMethod.PayPal, 150, "Address 456")
            }
        };

            return Task.FromResult(bills);
        }

        public override Task SaveBillsAsync(List<Bill> bills)
        {
            foreach (var bill in bills)
            {
                if (bill.Order == null)
                {
                    Console.WriteLine("Error: Bill is missing an associated order.");
                    return Task.CompletedTask;
                }
                else
                {
                    Console.WriteLine($"Bill {bill.BillId} saved for Order {bill.Order.OrderId}");
                }
            }
            return Task.CompletedTask;
        }
    }
}