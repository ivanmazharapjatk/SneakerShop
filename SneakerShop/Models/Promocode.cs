namespace SneakerShop.Models;

public class Promocode
{
    private static readonly List<Promocode> _extent = new List<Promocode>();
    public static IReadOnlyList<Promocode> Extent => _extent.AsReadOnly();
    
    private string _code;
    private int _numberOfUses;
    private decimal _discountPercent;
    private DateTime _startDate;
    private DateTime _endDate;

    public string Code
    {
        get => _code;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(Code), "Promocode cannot be empty.");

            if (value.Length > 15)
                throw new ArgumentException("Promocode must be no longer than 15 characters long.", nameof(Code));

            _code = value;
        }
    }

    public int NumberOfUses
    {
        get => _numberOfUses;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(NumberOfUses), "Number of uses cannot be negative.");

            _numberOfUses = value;
        }
    }

    public decimal DiscountPercent
    {
        get => _discountPercent;
        set
        {
            if (value < 5 || value > 30)
                throw new ArgumentOutOfRangeException(nameof(DiscountPercent),
                    "Discount percent must be between 5 and 30.");

            _discountPercent = value;
        }
    }

    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            if (value == default)
                throw new ArgumentException("Start date must be a valid date.", nameof(StartDate));

            if (_endDate != default && value >= _endDate)
                throw new ArgumentException("Start date must be earlier than end date.");

            _startDate = value;
        }
    }

    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            if (value == default)
                throw new ArgumentException("End date must be a valid date.", nameof(EndDate));

            if (_startDate != default && value <= _startDate)
                throw new ArgumentException("End date must be later than start date.");

            _endDate = value;
        }
    }

    public Promocode(string code, int numberOfUses, decimal discountPercent, DateTime startDate, DateTime endDate)
    {
        Code = code;
        NumberOfUses = numberOfUses;
        DiscountPercent = discountPercent;
        StartDate = startDate;
        EndDate = endDate;
        
        _extent.Add(this);
    }
}
