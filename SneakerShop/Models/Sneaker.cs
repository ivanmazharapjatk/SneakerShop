using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SneakerShop.Enums;

namespace SneakerShop.Models;

public class Sneaker : Product
{
    private static readonly List<Sneaker> _extent = new();
    public static IReadOnlyList<Sneaker> Extent => _extent.AsReadOnly();
    
    private const string ExtentFilePath = "SneakerExtent.json";
    
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

        var json = File.ReadAllText(ExtentFilePath);
        
        _ = JsonSerializer.Deserialize<List<Sneaker>>(json);
    }
    
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
    
    public Sneaker(
        string name,
        decimal price,
        ProductCategory category,
        bool available,
        string color,
        string material,
        string collection,
        int size
    ) : base()
    {
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
