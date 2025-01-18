using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public record PublishDeliveryCommand
    {
        public PublishDeliveryCommand(IReadOnlyCollection<UnvalidatedDelivery> inputShippings)
        {
            InputShippings = inputShippings;
        }

        public IReadOnlyCollection<UnvalidatedDelivery> InputShippings { get; }
    }
}
