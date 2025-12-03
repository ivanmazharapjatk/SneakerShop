using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using SneakerShop.Enums;

namespace SneakerShop.Models;

public class Accessory : Product
{
    // CLASS EXTENT FOR ACCESSORY
    private static readonly List<Accessory> _extent = new();
    public static IReadOnlyList<Accessory> Extent => _extent.AsReadOnly();
    
    private const string ExtentFilePath = "AccessoryExtent.json";

    public static void ClearExtent()
    {
        _extent.Clear();
    }
    
    public static void SaveExtent()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(_extent, options);
        File.WriteAllText(ExtentFilePath, json);
    }
    
    public static void LoadExtent()
    {
        if (!File.Exists(ExtentFilePath))
        {
            return;
        }
        
        ClearExtent();
        Product.ClearExtent();

        var json = File.ReadAllText(ExtentFilePath);
        
        var _ = JsonSerializer.Deserialize<List<Accessory>>(json);
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
    
    [JsonIgnore]
    public Product[] Compatibilities { get; set; } = Array.Empty<Product>();

    public Accessory(
        string name,
        decimal price,
        ProductCategory category,
        bool available,
        string color,
        string material,
        string type,
        Product[]? compatibilities = null
    ) : base()
    {
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
