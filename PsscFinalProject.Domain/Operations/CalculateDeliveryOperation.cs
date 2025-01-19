using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static PsscFinalProject.Domain.Models.OrderDelivery;

namespace PsscFinalProject.Domain.Operations
{
    internal class CalculateDeliveryOperation : OrderDeliveryOperation
    {
        public override IOrderDelivery Transform(IOrderDelivery? delivery, object? state)
        {
            if (delivery == null)
            {
                throw new ArgumentNullException(nameof(delivery), "Delivery cannot be null.");
            }

            return delivery switch
            {
                ValidatedOrderDelivery validatedDelivery => OnValidated(validatedDelivery),
                _ => throw new InvalidOperationException("Unexpected delivery type.")
            };
        }

        protected override IOrderDelivery OnValidated(ValidatedOrderDelivery validatedDelivery)
        {
            var calculatedDeliveries = validatedDelivery.DeliveryList.Select(delivery =>
            {
                var calculatedTrackingNumber = new CalculatedTrackingNumber(delivery.TrackingNumber, delivery.Courier);
                return calculatedTrackingNumber;
            }).ToList();

            return new CalculatedOrderDelivery(calculatedDeliveries.AsReadOnly());
        }
    }
}
