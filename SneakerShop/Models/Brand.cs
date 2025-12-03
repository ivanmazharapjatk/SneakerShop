namespace SneakerShop.Models
{
    public class Brand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Collections { get; set; } = new();
        public string CountryOfOrigin { get; set; }
    }
}