using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public record Delivery
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }  
        public DateTime DeliveryDate { get; set; }
        public TrackingNumber? TrackingNumber { get; set; }
        public Courier? Courier { get; set; } 
    }
}
