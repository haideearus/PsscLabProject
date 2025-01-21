using PsscFinalProject.Domain.Models;

namespace WebUI.Models
{
    public class DeliveryViewModel
    {
        public int DeliveryId { get; set; } // Primary Key
        public int OrderId { get; set; }    // Foreign Key to Order
        public DateTime DeliveryDate { get; set; }
        public TrackingNumber? TrackingNumber { get; set; }
        public Courier? Courier { get; set; } 

        public DeliveryViewModel(int deliveryId, int orderId, DateTime deliveryDate, TrackingNumber trackingNumber, Courier courier)
        {
            DeliveryId = deliveryId;
            OrderId = orderId;
            DeliveryDate = deliveryDate;
            TrackingNumber = trackingNumber;
            Courier = courier;
        }

        public DeliveryViewModel() { }
    }
}
