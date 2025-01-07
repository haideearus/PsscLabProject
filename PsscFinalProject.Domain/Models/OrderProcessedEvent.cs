using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public interface IOrderProcessedEvent { }

    // Success event
    public class OrderProcessedEvent : IOrderProcessedEvent
    {
        public int OrderId { get; }
        public string Message { get; }

        public OrderProcessedEvent(int orderId, string message)
        {
            OrderId = orderId;
            Message = message;
        }
    }

    // Failure event
    public class OrderProcessingFailedEvent : IOrderProcessedEvent
    {
        public string Reason { get; }

        public OrderProcessingFailedEvent(string reason)
        {
            Reason = reason;
        }
    }

}
