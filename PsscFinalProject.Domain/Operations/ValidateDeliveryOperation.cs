using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static PsscFinalProject.Domain.Models.OrderDelivery;

namespace PsscFinalProject.Domain.Operations
{
    internal class ValidateDeliveryOperation : OrderDeliveryOperation
    {
        public override IOrderDelivery Transform(IOrderDelivery? delivery, object? state)
        {
            if (delivery == null)
            {
                throw new ArgumentNullException(nameof(delivery), "Delivery cannot be null.");
            }

            return delivery switch
            {
                UnvalidatedOrderDelivery unvalidatedDelivery => OnUnvalidated(unvalidatedDelivery),
                _ => throw new InvalidOperationException("Unexpected delivery type.")
            };
        }

        protected override IOrderDelivery OnUnvalidated(UnvalidatedOrderDelivery unvalidatedDelivery)
        {
            var validationResults = unvalidatedDelivery.DeliveryList.Select(ValidateDelivery).ToList();
            var validatedDeliveries = validationResults
                .Where(result => result.IsValid && result.ValidatedDelivery != null) // Filter valid and non-null
                .Select(result => result.ValidatedDelivery!)
                .ToList();

            var validationErrors = validationResults
                .Where(result => !result.IsValid)
                .SelectMany(result => result.Errors)
                .ToList();

            if (validationErrors.Any())
            {
                return new InvalidOrderDelivery(unvalidatedDelivery.DeliveryList, validationErrors);
            }

            return new ValidatedOrderDelivery(validatedDeliveries.AsReadOnly());
        }

        private (bool IsValid, ValidatedDelivery? ValidatedDelivery, List<string> Errors) ValidateDelivery(UnvalidatedDelivery delivery)
        {
            var errors = new List<string>();

            if (!TrackingNumber.TryParse(delivery.TrackingNumber, out var trackingNumber))
            {
                errors.Add($"Invalid tracking number: {delivery.TrackingNumber}");
            }

            if (!Courier.TryParse(delivery.Courier, out var courier))
            {
                errors.Add($"Invalid courier: {delivery.Courier}");
            }

            if (!errors.Any() && trackingNumber != null && courier != null)
            {
                return (true, new ValidatedDelivery(trackingNumber, courier), errors);
            }

            return (false, null, errors);
        }
    }
}
