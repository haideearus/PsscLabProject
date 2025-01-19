using PsscFinalProject.Domain.Exceptions;
using static PsscFinalProject.Domain.Models.OrderDelivery;
using System;

namespace PsscFinalProject.Domain.Operations
{
    internal abstract class OrderDeliveryOperation : DomainOperation<IOrderDelivery, object, IOrderDelivery>
    {
        public override IOrderDelivery Transform(IOrderDelivery entity, object? state)
        {
            return entity switch
            {
                UnvalidatedOrderDelivery unvalidated => OnUnvalidated(unvalidated),
                ValidatedOrderDelivery validated => OnValidated(validated),
                CalculatedOrderDelivery calculated => OnCalculated(calculated),
                _ => throw new InvalidOperationException($"Unsupported type: {entity.GetType().Name}")
            };
        }

        protected virtual IOrderDelivery OnUnvalidated(UnvalidatedOrderDelivery unvalidated) => unvalidated;

        protected virtual IOrderDelivery OnValidated(ValidatedOrderDelivery validated) => validated;

        protected virtual IOrderDelivery OnCalculated(CalculatedOrderDelivery calculated) => calculated;
    }
}
