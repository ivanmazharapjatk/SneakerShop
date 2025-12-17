using System;
using System.Collections.Generic;

namespace SneakerShop.Models
{
    public class Supply
    {
        private static readonly List<Supply> _extent = new();
        public static IReadOnlyList<Supply> Extent => _extent.AsReadOnly();

        public DateTime SupplyDate { get; private set; }
        public Stock Stock { get; private set; }
        public Supplier Supplier { get; private set; }

        private Supply(Stock stock, Supplier supplier, DateTime supplyDate)
        {
            Stock = stock ?? throw new ArgumentNullException(nameof(stock));
            Supplier = supplier ?? throw new ArgumentNullException(nameof(supplier));
            SupplyDate = supplyDate != default
                ? supplyDate
                : throw new ArgumentException("Supply date must be a valid date.", nameof(supplyDate));

            RegisterAssociation();
            _extent.Add(this);
        }

        public static Supply Create(Stock stock, Supplier supplier, DateTime supplyDate)
        {
            return new Supply(stock, supplier, supplyDate);
        }

        public void Delete()
        {
            Stock.UnregisterSupply(this);
            Supplier.UnregisterSupply(this);
            _extent.Remove(this);
        }

        public static void ClearExtent()
        {
            foreach (var supply in _extent.ToArray())
            {
                supply.Delete();
            }
        }

        private void RegisterAssociation()
        {
            Stock.RegisterSupply(this);
            Supplier.RegisterSupply(this);
        }
    }
}
