using NUnit.Framework;
using SneakerShop.Models;

namespace SneakerShopTest;

[TestFixture]
public class SneakerShopUnitTests
{
    [SetUp]
    public void Setup()
    {
        Sneaker.ClearExtent();
        Accessory.ClearExtent();
        Employee.ClearExtent();
    }

    [Test]
    public void Customer_CanAddProductsToCartAndCalculateTotal()
    { 
        var sneaker = new Sneaker("Nike Air Force 1", 120m, "Lifestyle", true, "White", "Leather", "Classic", 42);
        var accessory = new Accessory("Shoe Cleaner", 15m, "Care", true, "Clear", "Chemical", "Cleaning", new Product[] { sneaker });
        
        var cartItems = new Product[] { sneaker, accessory };
        decimal total = cartItems.Sum(p => p.Price);

        Assert.Multiple(() =>
        {
            Assert.That(cartItems, Has.Length.EqualTo(2));
            Assert.That(total, Is.EqualTo(135m));
            Assert.That(sneaker.Available, Is.True, "Sneaker should be available for purchase");
        });
    }

    [Test]
    public void Employee_HighClearanceLevel_CanApproveRefunds()
    {
        var highLevelEmployee = new Employee("Manager", "Smith", "Store Manager", 3, new DateTime(2020, 1, 1));
        var lowLevelEmployee = new Employee("John", "Doe", "Cashier", 1, new DateTime(2023, 1, 1));

        Assert.Multiple(() =>
        {
            Assert.That(highLevelEmployee.ClearanceLevel, Is.EqualTo(3), "Manager should have high clearance");
            Assert.That(lowLevelEmployee.ClearanceLevel, Is.EqualTo(1), "Cashier should have low clearance");
        });
    }

    [Test]
    public void Customer_CanRatePurchasedSneaker_WithValidRating()
    {
        var sneaker = new Sneaker("Adidas Ultraboost", 180m, "Running", true, "Black", "Knit", "Performance", 43);

        sneaker.Rating = 4.5;

        Assert.That(sneaker.Rating, Is.EqualTo(4.5));
        Assert.DoesNotThrow(() => sneaker.Rating = 5.0, "Valid rating should not throw exception");
    }

    [Test]
    public void Customer_SubmitsInvalidRating_ThrowsException()
    {
        var sneaker = new Sneaker("Puma RS-X", 110m, "Lifestyle", true, "Red", "Suede", "Retro", 44);

        Assert.Throws<ArgumentOutOfRangeException>(() => sneaker.Rating = 6.0, 
            "Rating above 5 should throw exception");
    }
    

    [Test]
    public void Employee_SalaryCalculation_IncludesBaseAndBonus()
    {
        var employee = new Employee("Alice", "Johnson", "Senior Sales", 2, new DateTime(2021, 6, 15));
        
        employee.Bonus = 500m;
        decimal totalSalary = employee.SalaryNow;

        Assert.That(totalSalary, Is.EqualTo(Employee.BaseSalary + 500m));
    }

    [Test]
    public void Product_NotAvailable_CannotBePurchased()
    {
        var outOfStockSneaker = new Sneaker("Limited Edition", 300m, "Lifestyle", false, "Gold", "Leather", "Limited", 45);

        Assert.That(outOfStockSneaker.Available, Is.False, "Out of stock product should not be available");
    }

    [Test]
    public void Employee_Creation_WithValidData_Success()
    {
        var employee = new Employee("John", "Smith", "Logistics Coordinator", 2, new DateTime(2022, 1, 1));

        Assert.Multiple(() =>
        {
            Assert.That(employee.Name, Is.EqualTo("John"));
            Assert.That(employee.Surname, Is.EqualTo("Smith"));
            Assert.That(employee.Position, Is.EqualTo("Logistics Coordinator"));
            Assert.That(Employee.Extent, Contains.Item(employee));
        });
    }

    [Test]
    public void Sneaker_Creation_WithValidData_Success()
    {
        var sneaker = new Sneaker("New Balance 990", 175m, "Running", true, "Grey", "Suede", "Heritage", 44);

        Assert.Multiple(() =>
        {
            Assert.That(sneaker.Name, Is.EqualTo("New Balance 990"));
            Assert.That(sneaker.Price, Is.EqualTo(175m));
            Assert.That(sneaker.Size, Is.EqualTo(44));
            Assert.That(Sneaker.Extent, Contains.Item(sneaker));
        });
    }
    

    [Test]
    public void Employee_InvalidClearanceLevel_ThrowsException()
    {
        var hireDate = new DateTime(2020, 1, 15);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Employee("John", "Doe", "Manager", 5, hireDate));
    }

    [Test]
    public void Product_NegativePrice_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Sneaker("Test", -10m, "Test", true, "Black", "Leather", "Test", 42));
    }
}