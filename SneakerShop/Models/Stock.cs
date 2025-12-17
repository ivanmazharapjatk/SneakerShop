using System;
using System.Collections.Generic;

namespace SneakerShop.Models
{
    public class Stock
    {
        private readonly List<Supply> _supplies = new();
        public IReadOnlyList<Supply> Supplies => _supplies.AsReadOnly();
        public Supplier? Supplier { get; private set; }

        public int Quantity { get; set; }

        public int GetStock()
        {
            return Quantity;
        }

        internal void RegisterSupply(Supply supply)
        {
            if (supply == null) throw new ArgumentNullException(nameof(supply));

            if (Supplier != null && !ReferenceEquals(Supplier, supply.Supplier))
                throw new InvalidOperationException("Stock is already linked to a different supplier.");

            Supplier ??= supply.Supplier;

            if (!_supplies.Contains(supply))
            {
                _supplies.Add(supply);
            }
        }

        internal void UnregisterSupply(Supply supply)
        {
            if (supply == null) throw new ArgumentNullException(nameof(supply));

            _supplies.Remove(supply);

            if (_supplies.Count == 0)
            {
                Supplier = null;
            }
        }
    }
}
