using PsscFinalProject.Data.Repositories;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PsscFinalProject.Domain.Models
{
    // Assuming these records are already defined as per your structure:

    public interface ICartState
    {
    }

    //public class ShoppingCart
    //{
    //    public int Id { get; set; }
    //    public int ClientId { get; set; }
    //    public string ClientEmail { get; set; }
    //    public ICartState State { get; set; }  // Current state of the cart

    //    private readonly IClientRepository clientRepository; // Client repository to fetch clients

    //    // Constructor to initialize client repository
    //    public ShoppingCart(int clientId, string clientEmail, IClientRepository clientRepository)
    //    {
    //        ClientId = clientId;
    //        ClientEmail = clientEmail;
    //        this.clientRepository = clientRepository; // Inject the repository
    //        State = new EmptyShoppingCart(); // Initially, the cart is empty
    //    }

    //    // Transition from Empty to Unvalidated state after adding items
    //    public ShoppingCart AddProduct(List<string> items)
    //    {
    //        if (State is not EmptyShoppingCart)
    //        {
    //            throw new InvalidOperationException("Products can only be added to an empty cart.");
    //        }

    //        var newState = new UnvalidatedShoppingCart(items);
    //        return new ShoppingCart(ClientId, ClientEmail, clientRepository) { State = newState };
    //    }

    //    // Transition from Unvalidated to Validated state after computing the total
    //    public ShoppingCart ComputeTotal(decimal totalAmount)
    //    {
    //        if (State is not UnvalidatedShoppingCart)
    //        {
    //            throw new InvalidOperationException("Cannot compute total unless the cart is unvalidated.");
    //        }

    //        var newState = new ValidatedShoppingCart(totalAmount);
    //        return new ShoppingCart(ClientId, ClientEmail, clientRepository) { State = newState };
    //    }

    //    // Transition from Validated to Paid state after payment
    //    public ShoppingCart Pay()
    //    {
    //        if (State is not ValidatedShoppingCart)
    //        {
    //            throw new InvalidOperationException("Payment can only be made on validated carts.");
    //        }

    //        var validatedState = (ValidatedShoppingCart)State;
    //        var newState = new PaidShoppingCart(validatedState.TotalAmount, DateTime.Now);
    //        return new ShoppingCart(ClientId, ClientEmail, clientRepository) { State = newState };
    //    }

    //    // Optionally, transition to order creation after payment
    //    public async Task<Order> PlaceOrderAsync(string shippingAddress)
    //    {
    //        if (State is not PaidShoppingCart)
    //        {
    //            throw new InvalidOperationException("Order can only be placed after the cart is paid.");
    //        }

    //        var paidState = (PaidShoppingCart)State;

    //        // Get the client by ClientId asynchronously
           

    //        // Create an Order based on the paid cart state
    //        return new Order(
    //            client,                   // Pass the retrieved Client object
    //            DateTime.Now,             // Set the order date to current date and time
    //            PaymentMethod.CreditCard, // Set payment method (or use the actual payment method used)
    //            paidState.TotalAmount,    // Set the total amount from the paid cart state
    //            shippingAddress           // Set the shipping address
    //        );
    //    }
    
}
