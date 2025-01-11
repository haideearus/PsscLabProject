using PsscFinalProject.Domain.Models;
using PsscFinalProject.Domain.Repositories;
using PsscFinalProject.Domain.Operations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.Order;
using static PsscFinalProject.Domain.Models.OrderPublishEvent;
using PsscFinalProject.Data.Repositories;

namespace PsscFinalProject.Domain.Workflows
{
    public class PublishOrderWorkflow
    {
        private readonly IClientRepository clientRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;
        private readonly ILogger<PublishOrderWorkflow> logger;

        public PublishOrderWorkflow(
            IClientRepository clientRepository,
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ILogger<PublishOrderWorkflow> logger)
        {
            this.clientRepository = clientRepository;
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
            this.logger = logger;
        }

        public async Task<IOrderPublishEvent> ExecuteAsync(PublishOrderCommand command)
        {
            try
            {
                // Load existing data
                IEnumerable<string> productCodesToCheck = command.InputOrders.SelectMany(order => order.ProductList.Select(product => product.ProductId));
                List<ProductCode> existingProducts = await productRepository.GetExistingProductsAsync(productCodesToCheck);

                // Get existing clients from the repository
                IEnumerable<string> clientEmailsToCheck = command.InputOrders.Select(order => order.ProductList.First().ProductId); // Assuming first product has the client info
                List<ClientEmail> existingClients = await clientRepository.GetExistingClientsAsync(clientEmailsToCheck);

                // Execute business logic on each order
                List<IOrderPublishEvent> events = new List<IOrderPublishEvent>();
                foreach (var order in command.InputOrders)
                {
                    // Null check for order
                    if (order == null)
                    {
                        throw new ArgumentNullException(nameof(order), "Order cannot be null");
                    }

                    IOrder orderState = ExecuteBusinessLogic(order, existingProducts, existingClients);

                    // Save the final state of the order
                    if (orderState is PaidOrder paidOrder)
                    {
                        await orderRepository.SaveOrdersAsync(paidOrder);
                    }

                    // Generate event based on the order state
                    IOrderPublishEvent orderEvent = orderState.ToEvent();
                    events.Add(orderEvent);
                }

                // Return the appropriate event(s)
                return events.Count == 1 ? events.First() : new OrderPublishFailedEvent(new List<string> { "Multiple orders failed or no valid order found." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while publishing orders");
                return new OrderPublishFailedEvent(new List<string> { "Unexpected error" });
            }
        }

        private IOrder ExecuteBusinessLogic(
     UnvalidatedOrder unvalidatedOrder,
     List<ProductCode> existingProducts,
     List<ClientEmail> existingClients)
        {
            // Validate the order
            Func<string, bool> checkProductExists = productId => existingProducts.Any(p => p.Equals(productId));
            var validateOrderOperation = new ValidateOrderOperation(checkProductExists);

            // Null check before transforming the order
            if (unvalidatedOrder == null)
            {
                throw new ArgumentNullException(nameof(unvalidatedOrder), "Order cannot be null");
            }

            IOrder validatedOrder = validateOrderOperation.Transform(unvalidatedOrder);

            // If the order is valid, calculate its totals
            if (validatedOrder is ValidatedOrder validOrder)
            {
                var calculateOrderOperation = new CalculateOrderOperation();
                IOrder calculatedOrder = calculateOrderOperation.Transform(validOrder, existingProducts);

                // Initialize PublishOrderOperation
                var publishOrderOperation = new PublishOrderOperation();

                if (calculatedOrder is CalculatedOrder calculated)
                {
                    return publishOrderOperation.Transform(calculated);
                }
                throw new InvalidOperationException("calculatedOrder is not a CalculatedOrder");
            }

            // If the order is invalid, return an invalid order state
            if (validatedOrder is InvalidOrder invalidOrder)
            {
                return invalidOrder; // No further processing needed, it is a failure
            }

            // Return the validated order if nothing else applies
            return validatedOrder;
        }
    }
}
