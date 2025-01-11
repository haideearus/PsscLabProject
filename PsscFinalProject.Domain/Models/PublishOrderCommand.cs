using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PsscFinalProject.Domain.Models.Order;

namespace PsscFinalProject.Domain.Models
{
    public record PublishOrderCommand
    {
        public PublishOrderCommand(IReadOnlyCollection<UnvalidatedOrder> inputOrders)
        {
           InputOrders= inputOrders;
        }
        public IReadOnlyCollection<UnvalidatedOrder> InputOrders { get; }
    }

  
}
