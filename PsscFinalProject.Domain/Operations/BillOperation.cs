using PsscFinalProject.Domain.Exceptions;
using static PsscFinalProject.Domain.Models.OrderBilling;
using System;

namespace PsscFinalProject.Domain.Operations
{
    internal abstract class OrderBillingOperation : DomainOperation<IOrderBilling, object, IOrderBilling>
    {
        public override IOrderBilling Transform(IOrderBilling entity, object? state)
        {
            return entity switch
            {
                UnvalidatedOrderBilling unvalidated => OnUnvalidated(unvalidated),
                ValidatedOrderBilling validated => OnValidated(validated),
                CalculatedOrderBilling calculated => OnCalculated(calculated),
                _ => throw new InvalidOperationException($"Unsupported type: {entity.GetType().Name}")
            };
        }

        protected virtual IOrderBilling OnUnvalidated(UnvalidatedOrderBilling unvalidated) => unvalidated;

        protected virtual IOrderBilling OnValidated(ValidatedOrderBilling validated) => validated;

        protected virtual IOrderBilling OnCalculated(CalculatedOrderBilling calculated) => calculated;
    }
}
