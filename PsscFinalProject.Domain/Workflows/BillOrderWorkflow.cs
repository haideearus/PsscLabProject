//using Microsoft.EntityFrameworkCore;
//using PsscFinalProject.Data;
//using PsscFinalProject.Data.Models;
//using PsscFinalProject.Domain.Models;
//using PsscFinalProject.Domain.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PsscFinalProject.Domain.Workflows
//{
//    public class BillOrderWorkflow
//    {
//        private readonly PsscDbContext _context;

//        public BillOrderWorkflow(PsscDbContext context)
//        {
//            _context = context;
//        }

//        //public virtual async Task<List<Bill>> GetBillsAsync()
//        //{
//        //    var billDtos = await _context.Bills.ToListAsync();

//        //    var bills = billDtos.Select(billDto => new Bill
//        //    {
//        //        BillId = billDto.BillId,
//        //        OrderId = billDto.OrderId,
//        //        TotalAmount = billDto.TotalAmount,
//        //        BillingDate = billDto.BillingDate,
//        //        Order = new Order(
//        //            new Client(billDto.Order.Client.ClientId, billDto.Order.Client.Email),
//        //            billDto.Order.OrderDate,
//        //            (PaymentMethod)billDto.Order.PaymentMethod,
//        //            billDto.Order.TotalAmount,
//        //            billDto.Order.ShippingAddress)
//        //    }).ToList();

//        //    return bills;
//        //}

//        //public virtual async Task<Bill> GetBillByIdAsync(int id)
//        //{
//        //    var billDto = await _context.Bills.FindAsync(id);

//        //    if (billDto == null)
//        //    {
//        //        throw new KeyNotFoundException($"Bill with id {id} not found.");
//        //    }

//        //    var bill = new Bill
//        //    {
//        //        BillId = billDto.BillId,
//        //        OrderId = billDto.OrderId,
//        //        TotalAmount = billDto.TotalAmount,
//        //        BillingDate = billDto.BillingDate,
//        //        Order = new Order(
//        //            new Client(billDto.Order.Client.ClientId, billDto.Order.Client.Email),
//        //            billDto.Order.OrderDate,
//        //            (PaymentMethod)billDto.Order.PaymentMethod,
//        //            billDto.Order.TotalAmount,
//        //            billDto.Order.ShippingAddress
//        //        )
//        //    };

//        //    return bill;
//        //}

//        public virtual async Task SaveBillsAsync(List<Bill> bills)
//        {
//            var billDtos = bills.Select(bill => new BillDto
//            {
//                BillId = bill.BillId,
//                OrderId = bill.Order?.OrderId ?? 0,
//                TotalAmount = bill.TotalAmount,
//                BillingDate = bill.BillingDate,
//                Order = bill.Order != null ? new OrderDto
//                {
//                    OrderId = bill.Order.OrderId,
//                } : throw new InvalidOperationException("The invoice must be associated with an order.")

//            }).ToList();

//            _context.Bills.AddRange(billDtos);
//            await _context.SaveChangesAsync();
//        }
//    }
//}