using PsscFinalProject.Domain.Exceptions;
using System;
using static PsscFinalProject.Domain.Models.OrderProducts;

namespace PsscFinalProject.Domain.Operations
{
    // Base class (OrderOperation<TState>)
    public abstract class OrderOperation<TState> : DomainOperation<IOrderProducts, TState, IOrderProducts>
        where TState : class
    {
        public override IOrderProducts Transform(IOrderProducts? order, TState? state)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null");
            }

            return order switch
            {
                UnvalidatedOrderProducts unvalidatedOrder => OnUnvalidated(unvalidatedOrder, state),
                ValidatedOrderProducts validatedOrder => OnValid(validatedOrder, state),
                InvalidOrderProducts invalidOrder => OnInvalid(invalidOrder, state),
                CalculatedOrder calculatedOrder => OnCalculated(calculatedOrder, state),
                PaidOrderProducts paidOrder => OnPaid(paidOrder, state),
                _ => throw new InvalidOrderStateException($"Unsupported order type: {order.GetType().Name}")
            };
        }

        protected virtual IOrderProducts OnUnvalidated(UnvalidatedOrderProducts unvalidatedOrder, TState? state) => unvalidatedOrder;

        protected virtual IOrderProducts OnValid(ValidatedOrderProducts validatedOrder, TState? state) => validatedOrder;

        protected virtual IOrderProducts OnCalculated(CalculatedOrder calculatedOrder, TState? state) => calculatedOrder;

        protected virtual IOrderProducts OnPaid(PaidOrderProducts paidOrder, TState? state) => paidOrder;

        protected virtual IOrderProducts OnInvalid(InvalidOrderProducts invalidOrder, TState? state) => invalidOrder;

        // Make OnValidated virtual to allow overriding in derived class
        protected virtual IOrderProducts OnValidated(ValidatedOrderProducts validatedOrder) => validatedOrder;
    }

    // Derived class (OrderOperation)
    public abstract class OrderOperation : OrderOperation<object>
    {
        internal IOrderProducts Transform(IOrderProducts order) => Transform(order, null);

        protected sealed override IOrderProducts OnUnvalidated(UnvalidatedOrderProducts unvalidatedOrder, object? state) => OnUnvalidated(unvalidatedOrder);

        protected virtual IOrderProducts OnUnvalidated(UnvalidatedOrderProducts unvalidatedOrder) => unvalidatedOrder;

        protected sealed override IOrderProducts OnValid(ValidatedOrderProducts validatedOrder, object? state) => OnValid(validatedOrder);

        protected virtual IOrderProducts OnValid(ValidatedOrderProducts validatedOrder) => validatedOrder;

        protected sealed override IOrderProducts OnCalculated(CalculatedOrder calculatedOrder, object? state) => OnCalculated(calculatedOrder);

        protected virtual IOrderProducts OnCalculated(CalculatedOrder calculatedOrder) => calculatedOrder;

        protected sealed override IOrderProducts OnPaid(PaidOrderProducts paidOrder, object? state) => OnPaid(paidOrder);

        protected virtual IOrderProducts OnPaid(PaidOrderProducts paidOrder) => paidOrder;

        protected sealed override IOrderProducts OnInvalid(InvalidOrderProducts invalidOrder, object? state) => OnInvalid(invalidOrder);

        protected virtual IOrderProducts OnInvalid(InvalidOrderProducts invalidOrder) => invalidOrder;

        // Here we add the override keyword to override the base method
        protected override IOrderProducts OnValidated(ValidatedOrderProducts validatedOrder) => validatedOrder;
    }

}
