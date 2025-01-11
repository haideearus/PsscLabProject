using PsscFinalProject.Domain.Exceptions;
using PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.Order;

namespace PsscFinalProject.Domain.Operations
{
    // Base class (OrderOperation<TState>)
    internal abstract class OrderOperation<TState> : DomainOperation<IOrder, TState, IOrder>
        where TState : class
    {
        public override IOrder Transform(IOrder? order, TState? state)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null");
            }

            return order switch
            {
                UnvalidatedOrder unvalidatedOrder => OnUnvalidated(unvalidatedOrder, state),
                ValidatedOrder validatedOrder => OnValid(validatedOrder, state),
                InvalidOrder invalidOrder => OnInvalid(invalidOrder, state),
                CalculatedOrder calculatedOrder => OnCalculated(calculatedOrder, state),
                PaidOrder paidOrder => OnPaid(paidOrder, state),
                _ => throw new InvalidOrderStateException($"Unsupported order type: {order.GetType().Name}")
            };
        }

        protected virtual IOrder OnUnvalidated(UnvalidatedOrder unvalidatedOrder, TState? state) => unvalidatedOrder;

        protected virtual IOrder OnValid(ValidatedOrder validatedOrder, TState? state) => validatedOrder;

        protected virtual IOrder OnCalculated(CalculatedOrder calculatedOrder, TState? state) => calculatedOrder;

        protected virtual IOrder OnPaid(PaidOrder paidOrder, TState? state) => paidOrder;

        protected virtual IOrder OnInvalid(InvalidOrder invalidOrder, TState? state) => invalidOrder;

        // Make OnValidated virtual to allow overriding in derived class
        protected virtual IOrder OnValidated(ValidatedOrder validatedOrder) => validatedOrder;
    }

    // Derived class (OrderOperation)
    internal abstract class OrderOperation : OrderOperation<object>
    {
        internal IOrder Transform(IOrder order) => Transform(order, null);

        protected sealed override IOrder OnUnvalidated(UnvalidatedOrder unvalidatedOrder, object? state) => OnUnvalidated(unvalidatedOrder);

        protected virtual IOrder OnUnvalidated(UnvalidatedOrder unvalidatedOrder) => unvalidatedOrder;

        protected sealed override IOrder OnValid(ValidatedOrder validatedOrder, object? state) => OnValid(validatedOrder);

        protected virtual IOrder OnValid(ValidatedOrder validatedOrder) => validatedOrder;

        protected sealed override IOrder OnCalculated(CalculatedOrder calculatedOrder, object? state) => OnCalculated(calculatedOrder);

        protected virtual IOrder OnCalculated(CalculatedOrder calculatedOrder) => calculatedOrder;

        protected sealed override IOrder OnPaid(PaidOrder paidOrder, object? state) => OnPaid(paidOrder);

        protected virtual IOrder OnPaid(PaidOrder paidOrder) => paidOrder;

        protected sealed override IOrder OnInvalid(InvalidOrder invalidOrder, object? state) => OnInvalid(invalidOrder);

        protected virtual IOrder OnInvalid(InvalidOrder invalidOrder) => invalidOrder;

        // Here we add the override keyword to override the base method
        protected override IOrder OnValidated(ValidatedOrder validatedOrder) => validatedOrder;
    }

}
