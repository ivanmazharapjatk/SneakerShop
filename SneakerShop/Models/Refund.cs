using System;
using System.Collections.Generic;

namespace SneakerShop.Models
{
    public class Refund
    {
        private static readonly List<Refund> _refunds = new();
        public static IReadOnlyList<Refund> Refunds => _refunds.AsReadOnly();

        public string Description { get; private set; }
        public Order? Order { get; private set; }
        public bool IsActive => Order != null;

        private Refund(Order order, string description)
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
            Description = description;
        }

        internal static Refund Create(Order order, string description)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Refund description cannot be empty.", nameof(description));
            if (!Order.Orders.Contains(order))
                throw new InvalidOperationException("Cannot create a refund for an order that does not exist.");

            var refund = new Refund(order, description);
            order.RegisterRefund(refund);
            _refunds.Add(refund);

            return refund;
        }

        internal void DetachFromOrder()
        {
            Order = null;
            _refunds.Remove(this);
        }

        public static void ClearRefunds()
        {
            _refunds.Clear();
        }

        public void ApproveRefund()
        {
            if (!IsActive) throw new InvalidOperationException("Cannot approve refund without an order.");
            // TODO: Add logic later
        }

        public void RequestRefund()
        {
            if (!IsActive) throw new InvalidOperationException("Cannot request refund without an order.");
            // TODO: Add logic later
        }
    }
}
