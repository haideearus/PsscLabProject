using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PsscFinalProject.Domain.Models
{
    public record PublishOrderCommand
    {
        public PublishOrderCommand(IReadOnlyCollection<UnvalidatedOrder> inputProducts)
        {
            InputProducts = inputProducts;
        }
        public IReadOnlyCollection<UnvalidatedOrder> InputProducts { get; }
    }

}
