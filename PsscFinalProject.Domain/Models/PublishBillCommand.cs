using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public record PublishBillCommand
    {
        public PublishBillCommand(IReadOnlyCollection<UnvalidatedBill> inputBill)
        {
            InputBills = inputBill;
        }

        public IReadOnlyCollection<UnvalidatedBill> InputBills { get; }
    }
}
