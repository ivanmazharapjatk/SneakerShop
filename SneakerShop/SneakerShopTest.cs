using NUnit.Framework;
using SneakerShop.Models;
using SneakerShop.Enums;

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
        Product.ClearExtent();
    }

    [Test]
    public void Customer_CanAddProductsToCartAndCalculateTotal()
    {
        var sneaker = new Sneaker("Nike Air Force 1", 120m, ProductCategory.Lifestyle, true, "White", "Leather", "Classic", 42);

        var accessory = new Accessory(
            "Shoe Cleaner",
            15m,
            ProductCategory.Care,
            true,
            "Clear",
            "Chemical",
            "Cleaning",
            new Product[] { sneaker }
        );

        var cartItems = new Product[] { sneaker, accessory };
        decimal total = cartItems.Sum(p => p.Price);

        Assert.Multiple(() =>
        {
            Assert.That(cartItems, Has.Length.EqualTo(2));
            Assert.That(total, Is.EqualTo(135m));
            Assert.That(sneaker.Available, Is.True);
        });
    }

    [Test]
    public void Employee_HighClearanceLevel_CanApproveRefunds()
    {
        var highLevelEmployee = new Employee("Manager", "Smith", "Store Manager", 3, new DateTime(2020, 1, 1));
        var lowLevelEmployee = new Employee("John", "Doe", "Cashier", 1, new DateTime(2023, 1, 1));

        Assert.Multiple(() =>
        {
            Assert.That(highLevelEmployee.ClearanceLevel, Is.EqualTo(3));
            Assert.That(lowLevelEmployee.ClearanceLevel, Is.EqualTo(1));
        });
    }

    [Test]
    public void Customer_CanRatePurchasedSneaker_WithValidRating()
    {
        var sneaker = new Sneaker("Adidas Ultraboost", 180m, ProductCategory.Running, true, "Black", "Knit", "Performance", 43);

        sneaker.Rating = 4.5;

        Assert.That(sneaker.Rating, Is.EqualTo(4.5));
        Assert.DoesNotThrow(() => sneaker.Rating = 5.0);
    }

    [Test]
    public void Customer_SubmitsInvalidRating_ThrowsException()
    {
        var sneaker = new Sneaker("Puma RS-X", 110m, ProductCategory.Lifestyle, true, "Red", "Suede", "Retro", 44);

        Assert.Throws<ArgumentOutOfRangeException>(() => sneaker.Rating = 6.0);
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
        var outOfStockSneaker = new Sneaker("Limited Edition", 300m, ProductCategory.Lifestyle, false, "Gold", "Leather", "Limited", 45);

        Assert.That(outOfStockSneaker.Available, Is.False);
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
        var sneaker = new Sneaker("New Balance 990", 175m, ProductCategory.Running, true, "Grey", "Suede", "Heritage", 44);

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
            new Sneaker("Test", -10m, ProductCategory.Other, true, "Black", "Leather", "Test", 42));
    }

    [Test]
    public void Customer_Creation_WithValidData_Success()
    {
        var customer = new Customer
        {
            Username = "user123",
            Name = "John Doe",
            Email = "john@example.com",
            PhoneNumber = "123456789",
        };

        customer.Wishlist.Add(new Sneaker("Test", 100m, ProductCategory.Other, true, "Black", "Leather", "Test", 42));
        customer.Cart.Add(new Sneaker("CartItem", 120m, ProductCategory.Other, true, "White", "Mesh", "Test", 43));

        Assert.Multiple(() =>
        {
            Assert.That(customer.Username, Is.EqualTo("user123"));
            Assert.That(customer.Name, Is.EqualTo("John Doe"));
            Assert.That(customer.Email, Is.EqualTo("john@example.com"));
            Assert.That(customer.PhoneNumber, Is.EqualTo("123456789"));
            Assert.That(customer.Wishlist, Has.Count.EqualTo(1));
            Assert.That(customer.Cart, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void Delivery_Creation_WithValidData_Success()
    {
        var delivery = new Delivery
        {
            TrackingNumber = "TRACK123",
            DeliveryAddress = "123 Main St",
            EstimatedTime = new DateTime(2025, 1, 1),
            BaseDeliveryFee = 9.99m
        };

        Assert.Multiple(() =>
        {
            Assert.That(delivery.TrackingNumber, Is.EqualTo("TRACK123"));
            Assert.That(delivery.DeliveryAddress, Is.EqualTo("123 Main St"));
            Assert.That(delivery.EstimatedTime, Is.EqualTo(new DateTime(2025, 1, 1)));
            Assert.That(delivery.BaseDeliveryFee, Is.EqualTo(9.99m));
        });
    }

    [Test]
    public void CustomerSupportAgent_Creation_Success()
    {
        var agent = new CustomerSupportAgent
        {
            ContactNumber = "555-1234"
        };

        Assert.That(agent.ContactNumber, Is.EqualTo("555-1234"));
    }

    [Test]
    public void LogisticsCoordinator_AssignedSupplies_Creation()
    {
        var supply = new Supply { SupplyDate = new DateTime(2024, 5, 10) };
        var coordinator = new LogisticsCoordinator();

        coordinator.AssignedSupplies.Add(supply);

        Assert.Multiple(() =>
        {
            Assert.That(coordinator.AssignedSupplies.Count, Is.EqualTo(1));
            Assert.That(coordinator.AssignedSupplies[0].SupplyDate, Is.EqualTo(new DateTime(2024, 5, 10)));
        });
    }

    [Test]
    public void Refund_Creation_WithDescription()
    {
        var refund = new Refund
        {
            Description = "Damaged item"
        };

        Assert.That(refund.Description, Is.EqualTo("Damaged item"));
    }

    [Test]
    public void Store_Creation_WithValidData_Success()
    {
        var store = new Store
        {
            Name = "Main Store",
            Address = "456 Shop Street"
        };

        Assert.Multiple(() =>
        {
            Assert.That(store.Name, Is.EqualTo("Main Store"));
            Assert.That(store.Address, Is.EqualTo("456 Shop Street"));
        });
    }

    [Test]
    public void Stock_GetStock_ReturnsQuantity()
    {
        var stock = new Stock { Quantity = 25 };

        Assert.That(stock.GetStock(), Is.EqualTo(25));
    }

    [Test]
    public void Supply_Creation_WithDate_Success()
    {
        var supply = new Supply
        {
            SupplyDate = new DateTime(2023, 12, 5)
        };

        Assert.That(supply.SupplyDate, Is.EqualTo(new DateTime(2023, 12, 5)));
    }

    [Test]
    public void Supplier_Creation_Success()
    {
        var supplier = new Supplier
        {
            Name = "Global Supplies Inc",
            Location = "Berlin"
        };

        Assert.Multiple(() =>
        {
            Assert.That(supplier.Name, Is.EqualTo("Global Supplies Inc"));
            Assert.That(supplier.Location, Is.EqualTo("Berlin"));
        });
    }

    [Test]
    public void WaterProofProduct_Creation_Success()
    {
        var wp = new WaterProofProduct
        {
            MembraneType = "GoreTex",
            WaterColumnMm = 15000
        };

        Assert.Multiple(() =>
        {
            Assert.That(wp.MembraneType, Is.EqualTo("GoreTex"));
            Assert.That(wp.WaterColumnMm, Is.EqualTo(15000));
        });
    }

    [Test]
    public void SummerProduct_Creation_Success()
    {
        var sp = new SummerProduct
        {
            BreathabilityScore = 8.5
        };

        Assert.That(sp.BreathabilityScore, Is.EqualTo(8.5));
    }

    [Test]
    public void WinterizedProduct_Creation_Success()
    {
        var wp = new WinterizedProduct
        {
            InsulationLevel = 3,
            TractionGrade = 5
        };

        Assert.Multiple(() =>
        {
            Assert.That(wp.InsulationLevel, Is.EqualTo(3));
            Assert.That(wp.TractionGrade, Is.EqualTo(5));
        });
    }

    [Test]
    public void Brand_Creation_Success()
    {
        var brand = new Brand
        {
            Name = "Adidas",
            Description = "Sportswear",
            CountryOfOrigin = "Germany"
        };

        brand.Collections.Add("Originals");
        brand.Collections.Add("Performance");

        Assert.Multiple(() =>
        {
            Assert.That(brand.Name, Is.EqualTo("Adidas"));
            Assert.That(brand.Description, Is.EqualTo("Sportswear"));
            Assert.That(brand.Collections, Has.Count.EqualTo(2));
            Assert.That(brand.CountryOfOrigin, Is.EqualTo("Germany"));
        });
    }

    [Test]
    public void Collection_Creation_Success()
    {
        var collection = new Collection
        {
            Name = "Originals",
            Brand = "Adidas",
            Description = "Classic line"
        };

        Assert.Multiple(() =>
        {
            Assert.That(collection.Name, Is.EqualTo("Originals"));
            Assert.That(collection.Brand, Is.EqualTo("Adidas"));
            Assert.That(collection.Description, Is.EqualTo("Classic line"));
        });
    }
}
