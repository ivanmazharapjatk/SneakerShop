using System;

namespace SneakerShop.Models
{
    public class Customer
    {
        #region Class Fields
        
        private string _username = string.Empty;
        
        #endregion
        
        #region Attribute Validation
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
        
        #endregion
        
        // Wasn't sure about these two, but they must be separate associations and not actual attributes, so they are in no regions yet.
        public List<Product> Wishlist { get; set; } = new();
        public List<Product> Cart { get; set; } = new();
        
        #region Order History Association
        
        private readonly List<Order> _orderHistory = new();
        public IReadOnlyList<Order> OrderHistory => _orderHistory.AsReadOnly();
        
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

        #endregion
        
        //TODO: (!!!) Extent fields and persistence logic accordingly, class constructor, all the data validation, fix the association.
    }
}
