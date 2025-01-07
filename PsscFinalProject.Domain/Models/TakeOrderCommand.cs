using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public class TakeOrderCommand
    {
        public int ClientId { get; }
        public string ClientEmail { get; }
        public List<string> ProductIds { get; }
        public string ShippingAddress { get; }

        public TakeOrderCommand(int clientId, string clientEmail, List<string> productIds, string shippingAddress)
        {
            ClientId = clientId;
            ClientEmail = clientEmail;
            ProductIds = productIds;
            ShippingAddress = shippingAddress;
        }
    }

  
}
