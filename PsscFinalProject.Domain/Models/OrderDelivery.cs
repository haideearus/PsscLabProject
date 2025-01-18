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
            //private List<ValidatedShipp> validatedShippings;

            //public ValidatedOrderDelivery(List<ValidatedShipp> validatedShippings)
            //{
            //    this.validatedShippings = validatedShippings;
            //}

            //internal ValidatedOrderDelivery(IReadOnlyCollection<ValidatedShipping> shippingList)
            //{
            //    ShippingList = shippingList;
            //}

            //public IReadOnlyCollection<ValidatedShipping> ShippingList { get; }
        }

        public record CalculatedOrderDelivery : IOrderDelivery
        {
            //internal CalculatedOrderDelivery(IReadOnlyCollection<CalculatedShipping> shippingList)
            //{
            //    ShippingList = shippingList;
            //}

            //public IReadOnlyCollection<CalculatedShipping> ShippingList { get; }
        }

        public record PublishedOrderDelivery : IOrderDelivery
        {
            //internal PublishedOrderDelivery(IReadOnlyCollection<CalculatedShipping> shippingList, string csv, DateTime publishedDate)
            //{
            //    ShippingList = shippingList;
            //    PublishedDate = publishedDate;
            //    Csv = csv;
            //}

            //public IReadOnlyCollection<CalculatedShipping> ShippingList { get; }
            //public DateTime PublishedDate { get; }
            //public string Csv { get; }
        }
    }
}

