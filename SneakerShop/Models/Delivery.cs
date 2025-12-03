namespace SneakerShop.Models
{
    public class Delivery
    {
        public string TrackingNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime EstimatedTime { get; set; }
        public decimal BaseDeliveryFee { get; set; }
    }
}