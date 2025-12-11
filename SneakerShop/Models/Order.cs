using System;
using System.Collections.Generic;
using SneakerShop.Enums;

namespace SneakerShop.Models
{
    public class Order
    {
        private static readonly List<Order> _orders = new();
        public static IReadOnlyList<Order> Orders => _orders.AsReadOnly();
        private static readonly Dictionary<string, List<Order>> _ordersByUsername = new(StringComparer.OrdinalIgnoreCase);
        public static IReadOnlyList<Order> GetOrdersByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.", nameof(username));

            return _ordersByUsername.TryGetValue(username, out var orders)
                ? orders.AsReadOnly()
                : Array.Empty<Order>();
        }

        private readonly List<Refund> _refunds = new();
        public IReadOnlyList<Refund> Refunds => _refunds.AsReadOnly();
        public Customer Customer { get; private set; }
        public DateTime OrderDate { get; set; }
        public string PaymentInfo { get; set; }
        public OrderStatus Status { get; set; }
        
        public decimal TotalAmount { get; private set; }
        
        private readonly List<Product> _products = new();
        public IReadOnlyList<Product> Products => _products.AsReadOnly();

        private Order(Customer customer)
        {
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));

            RegisterQualifiedAssociation(customer);
            _orders.Add(this);
            customer.RegisterOrder(this);
        }

        public static IReadOnlyList<Order> GetOrders() => Orders;

        public static Order CreateOrder(
            Customer customer,
            DateTime date,
            string paymentInfo,
            OrderStatus status,
            string? promo = null)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (string.IsNullOrWhiteSpace(customer.Username))
                throw new InvalidOperationException("Customer must have a username to create an order.");

            var order = new Order(customer)
            {
                OrderDate = date,
                PaymentInfo = paymentInfo,
                Status = status
            };

            foreach (var p in customer.Cart)
                order._products.Add(p);

            customer.Cart.Clear();
            order.CalculateTotal(promo);

            return order;
        }

        private void CalculateTotal(string? promocodeString)
        {
            decimal sum = 0m;
            foreach (var product in _products)
                sum += product.Price;
            
            if (!string.IsNullOrWhiteSpace(promocodeString))
            {
                var promo = Promocode.Extent
                    .FirstOrDefault(p => string.Equals(p.Code, promocodeString, StringComparison.OrdinalIgnoreCase));

                if (promo != null)
                {
                    bool validDates =
                        promo.StartDate <= DateTime.Now &&
                        promo.EndDate >= DateTime.Now;

                    if (validDates && promo.NumberOfUses > 0)
                    {
                        decimal discountMultiplier = 1 - (promo.DiscountPercent / 100m);
                        sum *= discountMultiplier;
                        
                        promo.NumberOfUses--;
                    }
                }
            }

            TotalAmount = sum;
        }

        public void ChangeCustomer(Customer newCustomer)
        {
            if (newCustomer == null) throw new ArgumentNullException(nameof(newCustomer));
            if (string.IsNullOrWhiteSpace(newCustomer.Username))
                throw new InvalidOperationException("Customer must have a username to be assigned to an order.");
            if (Customer == newCustomer) return;

            UnregisterQualifiedAssociation(Customer.Username);
            Customer.UnregisterOrder(this);
            Customer = newCustomer;
            RegisterQualifiedAssociation(newCustomer);
            newCustomer.RegisterOrder(this);
        }

        public Refund CreateRefund(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Refund description cannot be empty.", nameof(description));

            var refund = new Refund(this, description);
            _refunds.Add(refund);
            return refund;
        }
        
        internal void RemoveRefund(Refund refund)
        {
            if (refund == null) throw new ArgumentNullException(nameof(refund));

            // Correct ownership check
            if (refund.Order != this)
                throw new InvalidOperationException("Refund does not belong to this order.");

            _refunds.Remove(refund);
            Refund.RemoveFromExtent(refund);
            refund.InternalDelete();
        }

        internal void DeleteRefund(Refund refund)
        {
            if (!_refunds.Contains(refund))
                throw new InvalidOperationException("Refund does not belong to this order.");

            _refunds.Remove(refund);
            Refund.RemoveFromExtent(refund);
        }


        public void Delete()
        {
            foreach (var refund in _refunds.ToArray())
                DeleteRefund(refund);

            UnregisterQualifiedAssociation(Customer?.Username);
            Customer?.UnregisterOrder(this);
            Customer = null!;
            _orders.Remove(this);
        }

        public static void ClearOrders()
        {
            foreach (var order in _orders.ToArray())
                order.Delete();

            _ordersByUsername.Clear();
        }

        private void RegisterQualifiedAssociation(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Username))
                throw new InvalidOperationException("Customer must have a username to create an order.");

            if (!_ordersByUsername.TryGetValue(customer.Username, out var orders))
            {
                orders = new List<Order>();
                _ordersByUsername[customer.Username] = orders;
            }

            if (!orders.Contains(this))
            {
                orders.Add(this);
            }
        }

        private void UnregisterQualifiedAssociation(string? username)
        {
            if (string.IsNullOrWhiteSpace(username)) return;

            if (_ordersByUsername.TryGetValue(username, out var orders))
            {
                orders.Remove(this);
                if (orders.Count == 0)
                {
                    _ordersByUsername.Remove(username);
                }
            }
        }

        internal static void UpdateQualifiedAssociation(Customer customer, string? oldUsername, string newUsername)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (string.IsNullOrWhiteSpace(newUsername))
                throw new ArgumentException("Username cannot be empty.", nameof(newUsername));

            if (!string.IsNullOrWhiteSpace(oldUsername) &&
                _ordersByUsername.TryGetValue(oldUsername, out var oldOrders))
            {
                oldOrders.RemoveAll(o => ReferenceEquals(o.Customer, customer));
                if (oldOrders.Count == 0)
                {
                    _ordersByUsername.Remove(oldUsername);
                }
            }

            foreach (var order in _orders)
            {
                if (ReferenceEquals(order.Customer, customer))
                {
                    if (!_ordersByUsername.TryGetValue(newUsername, out var orders))
                    {
                        orders = new List<Order>();
                        _ordersByUsername[newUsername] = orders;
                    }

                    if (!orders.Contains(order))
                    {
                        orders.Add(order);
                    }
                }
            }
        }
    }
}
