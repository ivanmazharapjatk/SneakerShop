using System.Text.Json;
using System.Text.Json.Serialization;
using SneakerShop.Enums;

namespace SneakerShop.Models
{
    public class Store
    {
        #region Constants
        
        private const string ExtentFilePath = "StoreExtent.json";
        
        #endregion
        
        #region Extent Fields
        
        private static readonly List<Store> _extent = new();
        public static IReadOnlyList<Store> Extent => _extent.AsReadOnly();
        
        #endregion
        
        #region Class Fields
        
        public string Name { get; set; }
        public string Address { get; set; }
        
        #endregion
        
        #region Constructors
        
        public Store(string name, string address)
        {
            Name = name;
            Address = address;
            _extent.Add(this);
        }
        
        #endregion
        
        #region Persistence Logic
        
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
        
        public static void ClearExtent()
        {
            _extent.Clear();
        }
        
        #endregion
        
        //TODO: Implement the associations
    }
}