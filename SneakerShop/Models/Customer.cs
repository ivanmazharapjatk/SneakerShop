using System;
using System.Collections.Generic;

namespace SneakerShop.Models
{
    public class Customer
    {
        //Username -> Order (qualified association)
        private readonly Dictionary<string, List<Order>> _ordersByUsername = new();

        public IReadOnlyDictionary<string, List<Order>> OrdersByUsername =>
            _ordersByUsername;

        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }

        public List<Product> Wishlist { get; set; } = new();
        public List<Product> Cart { get; set; } = new();

        internal void RegisterOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (string.IsNullOrWhiteSpace(Username))
                throw new InvalidOperationException("Customer must have Username to register order.");

            if (!_ordersByUsername.ContainsKey(Username))
            {
                _ordersByUsername[Username] = new List<Order>();
            }

            if (!_ordersByUsername[Username].Contains(order))
            {
                _ordersByUsername[Username].Add(order);
            }
        }

        internal void UnregisterOrder(Order order)
        {
            if (order == null || string.IsNullOrWhiteSpace(Username))
                return;

            if (_ordersByUsername.TryGetValue(Username, out var orders))
            {
                orders.Remove(order);

                if (orders.Count == 0)
                {
                    _ordersByUsername.Remove(Username);
                }
            }
        }

        //qualified access
        public IReadOnlyList<Order> GetOrdersByUsername(string username)
        {
            if (_ordersByUsername.TryGetValue(username, out var orders))
                return orders.AsReadOnly();

            return Array.Empty<Order>();
        }
    }
}
