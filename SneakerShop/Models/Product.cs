using SneakerShop.Enums;

namespace SneakerShop.Models
{
    public abstract class Product
    {
        #region Extent Fields
        
        // CLASS EXTENT FOR ALL PRODUCTS (all subclasses)
        private static readonly List<Product> _extent = new();
        public static IReadOnlyList<Product> Extent => _extent.AsReadOnly();

        #endregion
        
        #region Class Fields
        
        private string _name;
        private string _color;
        private string _material;
        private decimal _price;
        private double? _rating;
        private readonly List<Review> _reviews = new();
        
        #endregion
        
        #region Constructors
        
        protected Product()
        {
            _extent.Add(this);
        }
        
        #endregion
        
        #region Persistence Logic
        
        //IMPORTANT: We don't have a ProductExtent.json file since it's an abstract class and we only
        //extent instances of classes following it (such as Accessory and Sneaker).
        protected internal static void RemoveFromExtent(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            _extent.Remove(product);
        }

        public static void ClearExtent()
        {
            _extent.Clear();
        }
        
        #endregion
        
        #region Attribute Properties and Validation

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
        
        public ProductCategory Category { get; set; }

        #endregion
        
        #region Product-Review Association
        
        public IReadOnlyList<Review> Reviews => _reviews.AsReadOnly();
        
        public void AddReview(Review review)
        {
            if (review == null) throw new ArgumentNullException(nameof(review));
            _reviews.Add(review);
        }

        public double? GetRating()
        {
            if (_reviews.Count == 0)
            {
                Rating = null;
                return Rating;
            }

            double total = 0;
            foreach (var review in _reviews)
            {
                total += review.Rating;
            }

            Rating = total / _reviews.Count;
            return Rating;
        }
        
        #endregion
        
        #region Customer-Product Association
        
        public virtual void AddProduct() { }

        public virtual void AddProductToWishList(Customer customer)
        {
            customer.Wishlist.Add(this);
        }

        public virtual void AddProductToCart(Customer customer)
        {
            customer.Cart.Add(this);
        }
        
        #endregion
        
        //TODO: Fix association logic
    }
}
