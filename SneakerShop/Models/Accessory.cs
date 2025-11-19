namespace SneakerShop.Models;

public class Accessory : Product
{
    private static readonly List<Accessory> _extent = new List<Accessory>();
    public static IReadOnlyList<Accessory> Extent => _extent.AsReadOnly();
    
    public static void ClearExtent()
    {
        _extent.Clear();
    }
    
    private string _type;

    public string Type
    {
        get => _type;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Type cannot be empty.");
            _type = value;
        }
    }

    public Product[] Compatibilities { get; set; } = Array.Empty<Product>();
    
    public Accessory(string name, decimal price,
        string category, bool available,
        string color, string material,
        string type, Product[]? compatibilities = null) {
        Name = name;
        Price = price;
        Category = category;
        Available = available;
        Color = color;
        Material = material;
        Type = type;
        Compatibilities = compatibilities ?? Array.Empty<Product>();
        
        _extent.Add(this);
    }
}