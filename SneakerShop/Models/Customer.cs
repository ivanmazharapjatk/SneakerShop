namespace SneakerShop.Models
{
    public class Customer
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<Order> OrderHistory { get; set; } = new();
        public string? PhoneNumber { get; set; }
        public List<Product> Wishlist { get; set; } = new();
        public List<Product> Cart { get; set; } = new();
    }
}