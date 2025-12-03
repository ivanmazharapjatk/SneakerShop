using SneakerShop.Enums;

namespace SneakerShop.Models
{
    public abstract class Product
    {
        // CLASS EXTENT FOR ALL PRODUCTS (all subclasses)
        private static readonly List<Product> _extent = new();
        public static IReadOnlyList<Product> Extent => _extent.AsReadOnly();

        public static void ClearExtent()
        {
            _extent.Clear();
        }

        protected Product()
        {
            _extent.Add(this);
        }

        private string _name;
        private string _color;
        private string _material;
        private decimal _price;
        private double? _rating;
        
        public ProductCategory Category { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Product name cannot be empty.");
                _name = value;
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Price), "Price cannot be negative.");
                _price = value;
            }
        }

        public bool Available { get; set; }
        
        public string Color
        {
            get => _color;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Color cannot be empty.");
                _color = value;
            }
        }
        
        public string Material
        {
            get => _material;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Material cannot be empty.");
                _material = value;
            }
        }

        public double? Rating
        {
            get => _rating;
            set
            {
                if (value is < 1 or > 5)
                    throw new ArgumentOutOfRangeException(nameof(Rating), "Rating must be between 1 and 5.");
                _rating = value;
            }
        }
        
        public virtual void AddProduct() { }

        public virtual void AddProductToWishList(Customer customer)
        {
            customer.Wishlist.Add(this);
        }

        public virtual void AddProductToCart(Customer customer)
        {
            customer.Cart.Add(this);
        }
    }
}
