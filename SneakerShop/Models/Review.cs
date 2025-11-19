namespace SneakerShop.Models;

public class Review
{
    private static readonly List<Review> _extent = new List<Review>();
    public static IReadOnlyList<Review> Extent => _extent.AsReadOnly();

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