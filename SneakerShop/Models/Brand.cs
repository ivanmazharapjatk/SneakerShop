using System;
using System.Collections.Generic;

namespace SneakerShop.Models
{
    public class Brand
    {
        private static readonly List<Brand> _extent = new();
        public static IReadOnlyList<Brand> Extent => _extent.AsReadOnly();

        private readonly List<Sneaker> _sneakers = new();
        public IReadOnlyList<Sneaker> Sneakers => _sneakers.AsReadOnly();

        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Collections { get; set; } = new();
        public string CountryOfOrigin { get; set; }

        public Brand()
        {
            _extent.Add(this);
        }

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
    }
}
