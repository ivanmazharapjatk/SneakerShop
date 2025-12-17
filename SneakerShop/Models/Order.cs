using System;
using SneakerShop.Enums;

namespace SneakerShop.Models
{
    public class Order
    {
        private static readonly List<Order> _orders = new();
        public static IReadOnlyList<Order> Orders => _orders.AsReadOnly();

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
            _orders.Add(this);
            customer.RegisterOrder(this);
        }

        public static IReadOnlyList<Order> GetOrders() => Orders;
        
        public static Order CreateOrder(
            Customer customer,
            DateTime date,
            string paymentInfo,
            OrderStatus status,
            string? promocodeString = null)
        {
            if (customer == null) 
                throw new ArgumentNullException(nameof(customer));

            var order = new Order(customer)
            {
                OrderDate = date,
                PaymentInfo = paymentInfo,
                Status = status
            };
            
            foreach (var product in customer.Cart)
                order._products.Add(product);
            
            customer.Cart.Clear();
            
            order.CalculateTotal(promocodeString);

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
            if (newCustomer == null)
                throw new ArgumentNullException(nameof(newCustomer));

            if (Customer == newCustomer)
                return;

            Customer.UnregisterOrder(this);
            Customer = newCustomer;
            newCustomer.RegisterOrder(this);
        }

        public Refund CreateRefund(string description)
        {
            return Refund.Create(this, description);
        }

        internal void RegisterRefund(Refund refund)
        {
            if (refund == null) throw new ArgumentNullException(nameof(refund));
            _refunds.Add(refund);
        }

        internal void RemoveRefund(Refund refund)
        {
            if (refund == null) throw new ArgumentNullException(nameof(refund));
            if (!_refunds.Contains(refund))
                throw new InvalidOperationException("Refund does not belong to this order.");

            _refunds.Remove(refund);
            refund.DetachFromOrder();
        }

        public void Delete()
        {
            foreach (var refund in _refunds.ToArray())
                RemoveRefund(refund);

            Customer?.UnregisterOrder(this);
            Customer = null!;
            _orders.Remove(this);
        }

        public static void ClearOrders()
        {
            foreach (var order in _orders.ToArray())
                order.Delete();
        }
    }
}
