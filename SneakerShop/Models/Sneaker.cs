namespace SneakerShop.Models;

public class Sneaker : Product
{
    private static readonly List<Sneaker> _extent = new List<Sneaker>();
    public static IReadOnlyList<Sneaker> Extent => _extent.AsReadOnly();
    
    private string _collection;
    private int _size;

    public string Collection
    {
        get => _collection;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Collection cannot be empty.");
            _collection = value;
        }
    }

    public int Size
    {
        get => _size;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(Size), "Size must be a positive number.");
            _size = value;
        }
    }
    
    public Sneaker(string name, decimal price,
        string category, bool available,
        string color, string material,
        string collection, int size) {
        Name = name;
        Price = price;
        Category = category;
        Available = available;
        Color = color;
        Material = material;
        Collection = collection;
        Size = size; 
        
        _extent.Add(this);
    }
}