using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public record CalculatedBillNumber(ShippingAddress ShippingAddress, BillNumber BillNumber, ProductPrice ProductPrice)
    {
        public int BillId { get; set; }
        public int OrderId {  get; set; }
        public bool IsUpdated{ get; set; }
    }
}
