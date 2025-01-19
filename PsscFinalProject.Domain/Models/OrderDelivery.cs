using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public static partial class OrderDelivery
    {
        public interface IOrderDelivery { }

        public record UnvalidatedOrderDelivery : IOrderDelivery
        {
            public UnvalidatedOrderDelivery(IReadOnlyCollection<UnvalidatedDelivery> deliveryList)
            {
                DeliveryList = deliveryList;
            }

            public IReadOnlyCollection<UnvalidatedDelivery> DeliveryList { get; }
        }

        public record InvalidOrderDelivery : IOrderDelivery
        {
            internal InvalidOrderDelivery(IReadOnlyCollection<UnvalidatedDelivery> deliveryList, IEnumerable<string> reasons)
            {
                DeliveryList = deliveryList ?? throw new ArgumentNullException(nameof(deliveryList));
                Reasons = reasons ?? throw new ArgumentNullException(nameof(reasons));
            }

            public IReadOnlyCollection<UnvalidatedDelivery> DeliveryList { get; }
            public IEnumerable<string> Reasons { get; }
        }

        public record FailedOrderDelivery : IOrderDelivery
        {
            internal FailedOrderDelivery(IReadOnlyCollection<UnvalidatedDelivery> deliveryList, Exception exception)
            {
                DeliveryList = deliveryList ?? throw new ArgumentNullException(nameof(deliveryList));
                Exception = exception ?? throw new ArgumentNullException(nameof(exception));
            }

            public IReadOnlyCollection<UnvalidatedDelivery> DeliveryList { get; }
            public Exception Exception { get; }
        }

        public record ValidatedOrderDelivery : IOrderDelivery
        {

            internal ValidatedOrderDelivery(IReadOnlyCollection<ValidatedDelivery> deliveryList)
            {
                DeliveryList = deliveryList;
            }

            public IReadOnlyCollection<ValidatedDelivery> DeliveryList { get; }
        }

        public record CalculatedOrderDelivery : IOrderDelivery
        {
            internal CalculatedOrderDelivery(IReadOnlyCollection<CalculatedTrackingNumber> deliveryList)
            {
                DeliveryList = deliveryList;
            }

            public IReadOnlyCollection<CalculatedTrackingNumber> DeliveryList { get; }
        }

        public record PublishedOrderDelivery : IOrderDelivery
        {
            internal PublishedOrderDelivery(IReadOnlyCollection<CalculatedTrackingNumber> deliveryList, string csv, DateTime publishedDate)
            {
                DeliveryList = deliveryList;
                PublishedDate = publishedDate;
                Csv = csv;
            }

            public IReadOnlyCollection<CalculatedTrackingNumber> DeliveryList { get; }
            public DateTime PublishedDate { get; }
            public string Csv { get; }
        }
    }
}

