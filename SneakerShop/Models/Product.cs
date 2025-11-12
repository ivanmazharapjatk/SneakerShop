namespace SneakerShop.Models
{
    public abstract class Product
    {
        private string _name;
        private string _category;
        private string _color;
        private string _material;
        private decimal _price;
        private double? _rating;

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

        public string Category
        {
            get => _category;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Category cannot be empty.");
                _category = value;
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
    }
}
