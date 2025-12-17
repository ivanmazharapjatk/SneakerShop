using System;
using System.Collections.Generic;

namespace SneakerShop.Models
{
    public class Supplier
    {
        private readonly List<Supply> _supplies = new();
        public IReadOnlyList<Supply> Supplies => _supplies.AsReadOnly();

        public string Name { get; set; }
        public string Location { get; set; }

        public Supply SupplyStock(Stock stock, DateTime supplyDate)
        {
            return Supply.Create(stock, this, supplyDate);
        }

        internal void RegisterSupply(Supply supply)
        {
            if (supply == null) throw new ArgumentNullException(nameof(supply));

            if (!_supplies.Contains(supply))
            {
                _supplies.Add(supply);
            }
        }

        internal void UnregisterSupply(Supply supply)
        {
            _supplies.Remove(supply);
        }

        public void AskForSupply()
        {
            // TODO: Logic later
        }
    }
}
