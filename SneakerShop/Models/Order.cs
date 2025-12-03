using SneakerShop.Enums;

namespace SneakerShop.Models
{
    public class Order
    {
        private static readonly List<Order> _orders = new();
        public static IReadOnlyList<Order> Orders => _orders.AsReadOnly();

        public DateTime OrderDate { get; set; }
        public string PaymentInfo { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }

        public Order()
        {
            _orders.Add(this);
        }
        
        public static IReadOnlyList<Order> GetOrders() => Orders;

        public static Order CreateOrder(DateTime date, string paymentInfo, OrderStatus status, decimal totalAmount)
        {
            return new Order
            {
                OrderDate = date,
                PaymentInfo = paymentInfo,
                Status = status,
                TotalAmount = totalAmount
            };
        }
    }
}