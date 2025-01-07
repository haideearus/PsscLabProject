using System;
using System.Collections.Generic;

namespace PsscFinalProject.Domain.Models
{
    public class Order
    {
        // Order Id
        public int OrderId { get; private set; }

        // Client associated with the order
        public int ClientId { get; private set; }
        public Client Client { get; private set; }

        // Order Date
        public DateTime OrderDate { get; private set; }

        // Payment Method
        public PaymentMethod PaymentMethod { get; private set; }

        // Total Amount
        public decimal TotalAmount { get; private set; }

        // Shipping Address
        public string ShippingAddress { get; private set; }

        // Order State
        public OrderState? State { get; private set; }

        // Bills, Deliveries, and OrderItems
        public ICollection<Bill> Bills { get; private set; }
        public ICollection<Delivery> Deliveries { get; private set; }
        public ICollection<OrderItem> OrderItems { get; private set; }

        // Constructor with Client as a required parameter
        public Order(Client client, DateTime orderDate, PaymentMethod paymentMethod, decimal totalAmount, string shippingAddress)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client)); // Ensure client is not null
            ClientId = client.ClientId; // Optionally set ClientId if necessary
            OrderDate = orderDate;
            PaymentMethod = paymentMethod;
            TotalAmount = totalAmount;
            ShippingAddress = shippingAddress;

            Bills = new List<Bill>();
            Deliveries = new List<Delivery>();
            OrderItems = new List<OrderItem>();
        }


        // Method to change the state of the order
        public void SetOrderState(OrderState state)
        {
            State = state;
        }

        // Add an item to the order
        public void AddOrderItem(OrderItem item)
        {
            OrderItems.Add(item);
        }

        // Add a bill to the order
        public void AddBill(Bill bill)
        {
            Bills.Add(bill);
        }

        // Add a delivery to the order
        public void AddDelivery(Delivery delivery)
        {
            Deliveries.Add(delivery);
        }

        // Calculate the total price (could include additional logic for calculations)
        public void UpdateTotalAmount()
        {
            TotalAmount = 0;
            foreach (var item in OrderItems)
            {
                TotalAmount += item.Price * item.Quantity;
            }
        }
    }

    // Enum for Payment Method
    public enum PaymentMethod
    {
        CreditCard,
        CashOnDelivery
    }

    // Enum for Order State
    public enum OrderState
    {
        Placed,
        Billed,
        Delivered
    }

    
}
