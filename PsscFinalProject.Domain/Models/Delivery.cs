using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        // other properties...
        public int OrderId { get; set; }    // Foreign Key to Order
        public DateTime DeliveryDate { get; set; }
        public string TrackingNumber { get; set; } = string.Empty;
        public string Courier { get; set; } = string.Empty;
    }
}
