using System;

namespace SneakerShop.Models
{
    public class Refund
    {
        private static readonly List<Refund> _refunds = new();
        public static IReadOnlyList<Refund> Refunds => _refunds.AsReadOnly();

        public static void ClearRefunds()
        {
            _refunds.Clear();
        }

        public string Description { get; private set; }
        public Order Order { get; private set; }

        internal Refund(Order order, string description)
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
            Description = description;
        }

        internal void InternalDelete()
        {
            Order = null!;
        }

        internal static void RemoveFromExtent(Refund refund)
        {
            _refunds.Remove(refund);
        }
        
        public void ApproveRefund()
        {
            if (Order == null)
                throw new InvalidOperationException("Refund has been deleted.");

            // TODO: business logic
        }

        public void RequestRefund()
        {
            if (Order == null)
                throw new InvalidOperationException("Refund has been deleted.");

            // TODO: business logic
        }
    }
}