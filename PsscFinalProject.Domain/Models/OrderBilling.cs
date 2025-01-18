using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.OrderBilling;

namespace PsscFinalProject.Domain.Models // structura ii buna trebuie adaptata la noi la baza de date 
{
    public static partial class OrderBilling
    {
        public interface IOrderBilling { }

        public record UnvalidatedOrderBilling : IOrderBilling
        {
            public UnvalidatedOrderBilling(IReadOnlyCollection<UnvalidatedBill> billList)
            {
                BillList = billList;
            }

            public IReadOnlyCollection<UnvalidatedBill> BillList { get; }
        }

        public record InvalidOrderBilling : IOrderBilling
        {
            internal InvalidOrderBilling(IReadOnlyCollection<UnvalidatedBill> billList, IEnumerable<string> reasons)
            {
                BillList = billList ?? throw new ArgumentNullException(nameof(billList));
                Reasons = reasons ?? throw new ArgumentNullException(nameof(reasons));
            }

            public IReadOnlyCollection<UnvalidatedBill> BillList { get; }
            public IEnumerable<string> Reasons { get; }
        }

        public record FailedOrderBilling : IOrderBilling
        {
            internal FailedOrderBilling(IReadOnlyCollection<UnvalidatedBill> billList, Exception exception)
            {
                BillList = billList;
                Exception = exception;
            }

            public IReadOnlyCollection<UnvalidatedBill> BillList { get; }
            public Exception Exception { get; }
        }

        public record ValidatedOrderBilling : IOrderBilling
        {
            internal ValidatedOrderBilling(IReadOnlyCollection<ValidatedBill> billsList)
            {
                BillList = billsList;
            }

            public IReadOnlyCollection<ValidatedBill> BillList { get; }
        }

        public record CalculatedOrderBilling : IOrderBilling
        {
            internal CalculatedOrderBilling(IReadOnlyCollection<CalculatedBillNumber> billsList)
            {
                BillList = billsList;
            }

            public IReadOnlyCollection<CalculatedBillNumber> BillList { get; }
        }

        public record PublishedOrderBilling : IOrderBilling
        {
            internal PublishedOrderBilling(IReadOnlyCollection<CalculatedBillNumber> billsList, string csv, DateTime publishedDate)
            {
                BillList = billsList;
                PublishedDate = publishedDate;
                Csv = csv;
            }

            public IReadOnlyCollection<CalculatedBillNumber> BillList { get; }
            public DateTime PublishedDate { get; }
            public string Csv { get; }
        }
    }
}
