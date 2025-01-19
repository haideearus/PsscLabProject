using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public record CalculatedTrackingNumber(TrackingNumber TrackingNumber, Courier Courier)
    {
         public int DeliveryId { get; set; }
        public int OrderId {  get; set; }
    }
}
