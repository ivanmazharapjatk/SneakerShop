namespace SneakerShop.Models
{
    public class Supplier
    {
        #region Extent Fields
        
        private readonly List<Supply> _supplies = new();
        public IReadOnlyList<Supply> Supplies => _supplies.AsReadOnly();

        #endregion
        
        #region Class Fields
        
        public string Name { get; set; }
        public string Location { get; set; }
        
        #endregion

        #region Constructors
        
        public Supply SupplyStock(Stock stock, DateTime supplyDate)
        {
            return Supply.Create(stock, this, supplyDate);
        }
        
        #endregion

        #region Supply Association
        
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
        
        #endregion
    }
}
