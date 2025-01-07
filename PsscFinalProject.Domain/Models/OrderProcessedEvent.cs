using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public interface IOrderProcessedEvent { }
    public class OrderProcessedEvent : IOrderProcessedEvent
    {
        public int OrderId { get; }
        public int ClientId { get; }
        public string Message { get; }

        public OrderProcessedEvent(int orderId, int clientId, string message)
        {
            OrderId = orderId;
            ClientId = clientId;
            Message = message;
        }
    }

}
