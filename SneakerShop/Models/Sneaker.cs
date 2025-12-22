using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using SneakerShop.Enums;

namespace SneakerShop.Models;

public class Sneaker : Product
{
    #region Constants
    
    private const string ExtentFilePath = "SneakerExtent.json";
    
    #endregion
    
    #region Extent Fields
    
    private static readonly List<Sneaker> _extent = new();
    public static IReadOnlyList<Sneaker> Extent => _extent.AsReadOnly();
    
    #endregion
    
    #region Class Fields
    
    private string _collection;
    private int _size;
    
    #endregion
    
    #region Constructors
    
    public Sneaker(
        string name,
        decimal price,
        ProductCategory category,
        bool available,
        string color,
        string material,
        string collection,
        int size,
        Brand brand
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
        AssignBrand(brand);

        _extent.Add(this);
    }
    
    #endregion

    #region Attribute Properties and Validation
    
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
    
    #endregion
    
    #region Persistence Logic
    
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
    
    public static void ClearExtent()
    {
        foreach (var sneaker in _extent.ToArray())
        {
            sneaker.Delete();
        }
    }
    
    #endregion

    #region Brand Association
    
    [JsonIgnore] public Brand Brand { get; private set; }
    
    public void AssignBrand(Brand brand)
    {
        if (brand == null) throw new ArgumentNullException(nameof(brand));
        if (Brand == brand) return;

        var previousBrand = Brand;
        if (previousBrand != null)
        {
            previousBrand.UnregisterSneaker(this);
        }

        Brand = brand;
        brand.RegisterSneaker(this);
    }

    private void DetachBrand()
    {
        if (Brand == null) return;

        var currentBrand = Brand;
        Brand = null!;
        currentBrand.UnregisterSneaker(this);
    }

    public void Delete()
    {
        DetachBrand();
        _extent.Remove(this);
        RemoveFromExtent(this);
    }
    
    #endregion
}
