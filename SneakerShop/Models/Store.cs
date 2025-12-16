using System.Text.Json;
using System.Text.Json.Serialization;
using SneakerShop.Enums;

namespace SneakerShop.Models
{
    public class Store
    {
        private static readonly List<Store> _extent = new();
        public static IReadOnlyList<Store> Extent => _extent.AsReadOnly();
        private const string ExtentFilePath = "StoreExtent.json";
        public string Name { get; set; }
        public string Address { get; set; }
        
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
        
            _ = JsonSerializer.Deserialize<List<Store>>(json);
        }

        public Store(string name, string address)
        {
            Name = name;
            Address = address;
            _extent.Add(this);
        }
    }
}