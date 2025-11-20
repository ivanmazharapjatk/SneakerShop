using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SneakerShop.Models;

public class Review
{
    private static readonly List<Review> _extent = new();
    public static IReadOnlyList<Review> Extent => _extent.AsReadOnly();

    private const string ExtentFilePath = "ReviewExtent.json";

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
        
        _ = JsonSerializer.Deserialize<List<Review>>(json);
    }

    private int _rating;
    private string _header = "";
    private string _comment = "";

    public int Rating
    {
        get => _rating;
        set
        {
            if (value < 1 || value > 5)
                throw new ArgumentOutOfRangeException(nameof(Rating),
                    "Rating must be between 1 and 5.");

            _rating = value;
        }
    }

    public string Header
    {
        get => _header;
        set
        {
            if (_header.Length > 100)
                throw new ArgumentOutOfRangeException(nameof(Header), "Header cannot be longer than 100 characters.");
            

            if (value is null)
                throw new ArgumentNullException(nameof(Header), "Header cannot be null.");

            _header = value;
        }
    }

    public string Comment
    {
        get => _comment;
        set
        {
            if (_comment.Length > 500)
                throw new ArgumentOutOfRangeException(nameof(Comment), "Comment cannot be longer than 500 characters.");
            if (value is null)
                throw new ArgumentNullException(nameof(Comment), "Comment cannot be null.");

            _comment = value;
        }
    }

    public Review(int rating, string header, string comment)
    {
        Rating = rating;
        Header = header;
        Comment = comment;

        _extent.Add(this);
    }
}
