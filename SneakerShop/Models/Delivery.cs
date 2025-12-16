using System.Text.Json;

namespace SneakerShop.Models
{
    public class Delivery
    {
        private static readonly List<Delivery> _extent = new();
        public static IReadOnlyList<Delivery> Extent => _extent.AsReadOnly();
        private const string ExtentFilePath = "DeliveryExtent.json";
        public string TrackingNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime EstimatedTime { get; set; }
        public decimal BaseDeliveryFee { get; set; }
        
        public static void ClearExtent()
        {
            _extent.Clear();
        }

        public static void SaveExtent()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(_extent, options);
            File.WriteAllText(ExtentFilePath, json);
        }

        public static void LoadExtent()
        {
            if (!File.Exists(ExtentFilePath))
            {
                return;
            }

            ClearExtent();

            var json = File.ReadAllText(ExtentFilePath);
        
            _ = JsonSerializer.Deserialize<List<Delivery>>(json);
        }

        public Delivery(string trackingNumber, string deliveryAddress, DateTime estimatedTime, decimal baseDeliveryFee)
        {
            TrackingNumber = trackingNumber;
            DeliveryAddress = deliveryAddress;
            EstimatedTime = estimatedTime;
            BaseDeliveryFee = baseDeliveryFee;
            _extent.Add(this);
        }
    }
}