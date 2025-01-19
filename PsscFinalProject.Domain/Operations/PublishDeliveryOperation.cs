using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static PsscFinalProject.Domain.Models.OrderDelivery;

namespace PsscFinalProject.Domain.Operations
{
    internal class PublishDeliveryOperation : OrderDeliveryOperation
    {
        public override IOrderDelivery Transform(IOrderDelivery? delivery, object? state)
        {
            if (delivery == null)
            {
                throw new ArgumentNullException(nameof(delivery), "Delivery cannot be null.");
            }

            return delivery switch
            {
                CalculatedOrderDelivery calculatedDelivery => OnCalculated(calculatedDelivery),
                _ => throw new InvalidOperationException("Unexpected delivery type.")
            };
        }

        protected override IOrderDelivery OnCalculated(CalculatedOrderDelivery calculatedDelivery)
        {
            var csvLines = new List<string>
            {
                "TrackingNumber,Courier"
            };

            csvLines.AddRange(calculatedDelivery.DeliveryList.Select(delivery =>
                $"{delivery.TrackingNumber.Value},{delivery.Courier.Value}"));

            var csv = string.Join(Environment.NewLine, csvLines);
            var publishedDate = DateTime.UtcNow;

            return new PublishedOrderDelivery(calculatedDelivery.DeliveryList, csv, publishedDate);
        }
    }
}
