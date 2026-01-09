namespace SneakerShop.Models
{
    public class Brand
    {
        #region Extent Fields
        
        private static readonly List<Brand> _extent = new();
        public static IReadOnlyList<Brand> Extent => _extent.AsReadOnly();

        #endregion
        
        #region Class Fields
        
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Collections { get; set; } = new();
        public string CountryOfOrigin { get; set; }

        #endregion
        
        #region Constructors
        
        public Brand(string name, string description, string countryOfOrigin)
        {
            Name = name;
            Description = description;
            CountryOfOrigin = countryOfOrigin;
            
            _extent.Add(this);
        }
        
        public Brand() //this is a temporary empty constructor since I don't want to break old unit tests that use it
        {
            throw new NotImplementedException();
        } 

        #endregion
        
        #region Sneaker Association 
        
        private readonly List<Sneaker> _sneakers = new();

        public IReadOnlyList<Sneaker> Sneakers => _sneakers.AsReadOnly();
        
        public void AddSneaker(Sneaker sneaker)
        {
            if (sneaker == null) throw new ArgumentNullException(nameof(sneaker));
            sneaker.AssignBrand(this);
        }

        public void RemoveSneaker(Sneaker sneaker)
        {
            if (sneaker == null) throw new ArgumentNullException(nameof(sneaker));
            if (sneaker.Brand != this)
                throw new InvalidOperationException("Sneaker is not associated with this brand.");

            throw new InvalidOperationException("Reassign the sneaker to another brand or delete it before removal.");
        }

        internal void RegisterSneaker(Sneaker sneaker)
        {
            if (!_sneakers.Contains(sneaker))
            {
                _sneakers.Add(sneaker);
            }
        }

        internal void UnregisterSneaker(Sneaker sneaker)
        {
            _sneakers.Remove(sneaker);
        }

        public void Delete()
        {
            if (_sneakers.Count > 0)
                throw new InvalidOperationException("Cannot delete brand that still has sneakers.");

            _extent.Remove(this);
        }

        public static void ClearBrands()
        {
            Sneaker.ClearExtent();
            _extent.Clear();
        }
        
        #endregion
        
        //TODO: Persistence file for this class and attribute logic validation. Also fix the association obviously.
    }
}
