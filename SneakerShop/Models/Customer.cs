using System;

namespace SneakerShop.Models
{
    public class Customer
    {
        private readonly List<Order> _orderHistory = new();
        public IReadOnlyList<Order> OrderHistory => _orderHistory.AsReadOnly();

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username cannot be empty.");
                if (string.Equals(_username, value, StringComparison.OrdinalIgnoreCase))
                    return;

                var previousUsername = _username;
                Order.UpdateQualifiedAssociation(this, previousUsername, value);
                _username = value;
            }
        }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public List<Product> Wishlist { get; set; } = new();
        public List<Product> Cart { get; set; } = new();

        internal void RegisterOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (!_orderHistory.Contains(order))
            {
                _orderHistory.Add(order);
            }
        }

        internal void UnregisterOrder(Order order)
        {
            _orderHistory.Remove(order);
        }
    }
}
