namespace WebUI.Models
{
    public class DeliveryViewModel
    {
        public int DeliveryId { get; set; } // Primary Key
        public int OrderId { get; set; }    // Foreign Key to Order
        public DateTime DeliveryDate { get; set; }
        public string TrackingNumber { get; set; } = string.Empty;
        public string Courier { get; set; } = string.Empty;

        public DeliveryViewModel(int deliveryId, int orderId, DateTime deliveryDate, string trackingNumber, string courier)
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
