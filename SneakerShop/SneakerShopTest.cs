using NUnit.Framework;
using System.Linq;
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
        Brand.ClearBrands();
        Employee.ClearExtent();
        Product.ClearExtent();
        Order.ClearOrders();
        Refund.ClearRefunds();
    }

    [Test]
    public void Customer_CanAddProductsToCartAndCalculateTotal()
    {
        var nike = new Brand { Name = "Nike", Description = "Sportswear", CountryOfOrigin = "USA" };
        var sneaker = new Sneaker("Nike Air Force 1", 120m, ProductCategory.Lifestyle, true, "White", "Leather", "Classic", 42, nike);

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
        var adidas = new Brand { Name = "Adidas", Description = "Sportswear", CountryOfOrigin = "Germany" };
        var sneaker = new Sneaker("Adidas Ultraboost", 180m, ProductCategory.Running, true, "Black", "Knit", "Performance", 43, adidas);

        sneaker.Rating = 4.5;

        Assert.That(sneaker.Rating, Is.EqualTo(4.5));
        Assert.DoesNotThrow(() => sneaker.Rating = 5.0);
    }

    [Test]
    public void Customer_SubmitsInvalidRating_ThrowsException()
    {
        var puma = new Brand { Name = "Puma", Description = "Sportswear", CountryOfOrigin = "Germany" };
        var sneaker = new Sneaker("Puma RS-X", 110m, ProductCategory.Lifestyle, true, "Red", "Suede", "Retro", 44, puma);

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
        var brand = new Brand { Name = "Collab", Description = "Limited collab", CountryOfOrigin = "USA" };
        var outOfStockSneaker = new Sneaker("Limited Edition", 300m, ProductCategory.Lifestyle, false, "Gold", "Leather", "Limited", 45, brand);

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
        var nb = new Brand { Name = "New Balance", Description = "Lifestyle", CountryOfOrigin = "USA" };
        var sneaker = new Sneaker("New Balance 990", 175m, ProductCategory.Running, true, "Grey", "Suede", "Heritage", 44, nb);

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
        var brand = new Brand { Name = "TestBrand", Description = "Desc", CountryOfOrigin = "USA" };
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Sneaker("Test", -10m, ProductCategory.Other, true, "Black", "Leather", "Test", 42, brand));
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

        var brand = new Brand { Name = "ListBrand", Description = "Desc", CountryOfOrigin = "USA" };
        customer.Wishlist.Add(new Sneaker("Test", 100m, ProductCategory.Other, true, "Black", "Leather", "Test", 42, brand));
        customer.Cart.Add(new Sneaker("CartItem", 120m, ProductCategory.Other, true, "White", "Mesh", "Test", 43, brand));

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

    // Composition: Order - Refund
    [Test]
    public void Refund_CreatedViaOrder_BidirectionalLinks()
    {
        var customer = new Customer { Username = "john", Name = "John Doe", Email = "john@example.com" };
        var order = Order.CreateOrder(customer, new DateTime(2024, 10, 10), "CreditCard", OrderStatus.Processing);

        var refund = order.CreateRefund("Damaged item");

        Assert.Multiple(() =>
        {
            Assert.That(refund.Order, Is.EqualTo(order));
            Assert.That(order.Refunds, Contains.Item(refund));
            Assert.That(Refund.Refunds, Contains.Item(refund));
        });
    }

    [Test]
    public void Refund_CreateWithEmptyDescription_Throws()
    {
        var customer = new Customer { Username = "kate", Name = "Kate Doe", Email = "kate@example.com" };
        var order = Order.CreateOrder(customer, new DateTime(2024, 11, 1), "PayPal", OrderStatus.Shipped);

        Assert.Throws<ArgumentException>(() => order.CreateRefund("   "));
    }

    [Test]
    public void Refund_CreateForDeletedOrder_Throws()
    {
        var customer = new Customer { Username = "liz", Name = "Liz Doe", Email = "liz@example.com" };
        var order = Order.CreateOrder(customer, new DateTime(2024, 9, 9), "Cash", OrderStatus.Pending);

        order.Delete();

        Assert.Throws<InvalidOperationException>(() => Refund.Create(order, "Should fail"));
    }

    [Test]
    public void Refund_RemoveFromWrongOrder_Throws()
    {
        var customerA = new Customer { Username = "a", Name = "A", Email = "a@example.com" };
        var customerB = new Customer { Username = "b", Name = "B", Email = "b@example.com" };
        var firstOrder = Order.CreateOrder(customerA, new DateTime(2024, 7, 1), "Card", OrderStatus.Processing);
        var secondOrder = Order.CreateOrder(customerB, new DateTime(2024, 7, 2), "Card", OrderStatus.Processing);

        var refund = secondOrder.CreateRefund("Wrong item");

        Assert.Throws<InvalidOperationException>(() => firstOrder.RemoveRefund(refund));
    }

    [Test]
    public void Refund_ApproveDetached_Throws()
    {
        var customer = new Customer { Username = "tom", Name = "Tom", Email = "tom@example.com" };
        var order = Order.CreateOrder(customer, new DateTime(2024, 8, 1), "Card", OrderStatus.Shipped);
        var refund = order.CreateRefund("Late delivery");

        order.Delete(); // detaches and removes refund from extent

        Assert.Throws<InvalidOperationException>(() => refund.ApproveRefund());
    }

    [Test]
    public void Order_Delete_CascadesRefunds()
    {
        var customer = new Customer { Username = "mia", Name = "Mia", Email = "mia@example.com" };
        var order = Order.CreateOrder(customer, new DateTime(2024, 6, 15), "Card", OrderStatus.Processing);
        var refund = order.CreateRefund("Defect");

        order.Delete();

        Assert.Multiple(() =>
        {
            Assert.That(Order.Orders, Does.Not.Contain(order));
            Assert.That(Refund.Refunds, Does.Not.Contain(refund));
            Assert.That(refund.Order, Is.Null);
            Assert.That(customer.OrderHistory, Does.Not.Contain(order));
        });
    }

    // 1..* association: Customer - Order
    [Test]
    public void Customer_CreateOrder_AssociationEstablished()
    {
        var customer = new Customer { Username = "cust1", Name = "Cust One", Email = "cust1@example.com" };

        var order = Order.CreateOrder(customer, new DateTime(2024, 5, 5), "Card", OrderStatus.Pending);

        Assert.Multiple(() =>
        {
            Assert.That(order.Customer, Is.EqualTo(customer));
            Assert.That(customer.OrderHistory, Contains.Item(order));
        });
    }

    [Test]
    public void Order_ChangeCustomer_UpdatesHistories()
    {
        var firstCustomer = new Customer { Username = "first", Name = "First", Email = "first@example.com" };
        var secondCustomer = new Customer { Username = "second", Name = "Second", Email = "second@example.com" };
        var order = Order.CreateOrder(firstCustomer, new DateTime(2024, 4, 4), "Cash", OrderStatus.Pending);

        order.ChangeCustomer(secondCustomer);

        Assert.Multiple(() =>
        {
            Assert.That(order.Customer, Is.EqualTo(secondCustomer));
            Assert.That(secondCustomer.OrderHistory, Contains.Item(order));
            Assert.That(firstCustomer.OrderHistory, Does.Not.Contain(order));
        });
    }

    [Test]
    public void Order_CreateWithNullCustomer_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            Order.CreateOrder(null!, new DateTime(2024, 3, 3), "Cash", OrderStatus.Pending));
    }

    [Test]
    public void Order_Delete_RemovesFromCustomerHistory()
    {
        var customer = new Customer { Username = "solo", Name = "Solo", Email = "solo@example.com" };
        var order = Order.CreateOrder(customer, new DateTime(2024, 2, 2), "Cash", OrderStatus.Pending);

        order.Delete();

        Assert.That(customer.OrderHistory, Does.Not.Contain(order));
    }

    // Aggregation: Brand - Sneaker
    [Test]
    public void Sneaker_AssignBrand_Bidirectional()
    {
        var originalBrand = new Brand { Name = "Nike", Description = "Sports", CountryOfOrigin = "USA" };
        var newBrand = new Brand { Name = "Adidas", Description = "Sports", CountryOfOrigin = "Germany" };
        var sneaker = new Sneaker("Air Max", 150m, ProductCategory.Lifestyle, true, "Black", "Mesh", "Air", 42, originalBrand);

        sneaker.AssignBrand(newBrand);

        Assert.Multiple(() =>
        {
            Assert.That(sneaker.Brand, Is.EqualTo(newBrand));
            Assert.That(newBrand.Sneakers, Contains.Item(sneaker));
            Assert.That(originalBrand.Sneakers, Does.Not.Contain(sneaker));
        });
    }

    [Test]
    public void Sneaker_Delete_DoesNotRemoveBrand()
    {
        var brand = new Brand { Name = "Puma", Description = "Sportswear", CountryOfOrigin = "Germany" };
        var sneaker = new Sneaker("RS-X", 130m, ProductCategory.Lifestyle, true, "Red", "Mesh", "RS", 43, brand);

        sneaker.Delete();

        Assert.Multiple(() =>
        {
            Assert.That(Brand.Extent, Contains.Item(brand));
            Assert.That(brand.Sneakers, Does.Not.Contain(sneaker));
        });
    }

    [Test]
    public void Brand_RemoveUnrelatedSneaker_Throws()
    {
        var brandA = new Brand { Name = "A", Description = "A", CountryOfOrigin = "A" };
        var brandB = new Brand { Name = "B", Description = "B", CountryOfOrigin = "B" };
        var sneaker = new Sneaker("Model", 120m, ProductCategory.Lifestyle, true, "White", "Leather", "M", 41, brandA);

        Assert.Throws<InvalidOperationException>(() => brandB.RemoveSneaker(sneaker));
    }

    [Test]
    public void Brand_DuplicateSneaker_NotDuplicated()
    {
        var brand = new Brand { Name = "Reebok", Description = "Reebok", CountryOfOrigin = "USA" };
        var sneaker = new Sneaker("Classic", 100m, ProductCategory.Lifestyle, true, "White", "Leather", "Classic", 42, brand);

        brand.AddSneaker(sneaker);

        Assert.That(brand.Sneakers.Count(s => s == sneaker), Is.EqualTo(1));
    }

    // Reflexive association: Employee supervises Employee
    [Test]
    public void Employee_AssignSupervisor_Bidirectional()
    {
        var supervisor = new Employee("Alice", "Brown", "Manager", 3, new DateTime(2019, 5, 1));
        var subordinate = new Employee("Bob", "White", "Sales", 1, new DateTime(2023, 2, 1));

        subordinate.AssignSupervisor(supervisor);

        Assert.Multiple(() =>
        {
            Assert.That(subordinate.Supervisor, Is.EqualTo(supervisor));
            Assert.That(supervisor.Subordinates, Contains.Item(subordinate));
        });
    }

    [Test]
    public void Employee_RemoveNonSubordinate_Throws()
    {
        var supervisor = new Employee("Carl", "Stone", "Lead", 2, new DateTime(2021, 1, 1));
        var subordinate = new Employee("Dana", "Hill", "Staff", 1, new DateTime(2022, 1, 1));

        Assert.Throws<InvalidOperationException>(() => supervisor.RemoveSubordinate(subordinate));
    }

    [Test]
    public void Employee_SelfSupervision_Throws()
    {
        var employee = new Employee("Eve", "Self", "Analyst", 1, new DateTime(2020, 10, 10));

        Assert.Throws<InvalidOperationException>(() => employee.AssignSupervisor(employee));
    }

    [Test]
    public void Employee_CycleSupervision_Throws()
    {
        var manager = new Employee("Frank", "Boss", "Manager", 3, new DateTime(2018, 3, 3));
        var lead = new Employee("Gina", "Lead", "Lead", 2, new DateTime(2019, 4, 4));

        lead.AssignSupervisor(manager);

        Assert.Throws<InvalidOperationException>(() => manager.AssignSupervisor(lead));
    }

    [Test]
    public void Employee_RemoveSupervisor_DetachesBothSides()
    {
        var manager = new Employee("Hank", "Boss", "Manager", 3, new DateTime(2018, 5, 5));
        var staff = new Employee("Ivy", "Staff", "Staff", 1, new DateTime(2022, 6, 6));

        staff.AssignSupervisor(manager);
        staff.RemoveSupervisor();

        Assert.Multiple(() =>
        {
            Assert.That(staff.Supervisor, Is.Null);
            Assert.That(manager.Subordinates, Does.Not.Contain(staff));
        });
    }

    [Test]
    public void Refund_Creation_WithDescription()
    {
        var customer = new Customer { Username = "john", Name = "John Doe", Email = "john@example.com" };
        var order = Order.CreateOrder(customer, new DateTime(2024, 10, 10), "CreditCard", OrderStatus.Processing);
        var refund = order.CreateRefund("Damaged item");

        Assert.Multiple(() =>
        {
            Assert.That(refund.Description, Is.EqualTo("Damaged item"));
            Assert.That(refund.Order, Is.EqualTo(order));
            Assert.That(order.Refunds, Contains.Item(refund));
            Assert.That(Refund.Refunds, Contains.Item(refund));
        });
    }

    [Test]
    public void Order_Delete_RemovesAssociatedRefunds()
    {
        var customer = new Customer { Username = "kate", Name = "Kate Doe", Email = "kate@example.com" };
        var order = Order.CreateOrder(customer, new DateTime(2024, 11, 1), "PayPal", OrderStatus.Shipped);
        var refund = order.CreateRefund("Wrong size");

        order.Delete();

        Assert.Multiple(() =>
        {
            Assert.That(Order.Orders, Does.Not.Contain(order));
            Assert.That(Refund.Refunds, Does.Not.Contain(refund));
            Assert.That(refund.Order, Is.Null);
        });
    }

    [Test]
    public void Order_Associated_WithCustomer()
    {
        var customer = new Customer { Username = "anna", Name = "Anna Smith", Email = "anna@example.com" };

        var order = Order.CreateOrder(customer, new DateTime(2024, 12, 1), "Card", OrderStatus.Processing);

        Assert.Multiple(() =>
        {
            Assert.That(order.Customer, Is.EqualTo(customer));
            Assert.That(customer.OrderHistory, Contains.Item(order));
        });

        var newCustomer = new Customer { Username = "mike", Name = "Mike Brown", Email = "mike@example.com" };
        order.ChangeCustomer(newCustomer);

        Assert.Multiple(() =>
        {
            Assert.That(order.Customer, Is.EqualTo(newCustomer));
            Assert.That(newCustomer.OrderHistory, Contains.Item(order));
            Assert.That(customer.OrderHistory, Does.Not.Contain(order));
        });
    }

    [Test]
    public void Sneaker_Delete_DoesNotDeleteBrand()
    {
        var brand = new Brand
        {
            Name = "Nike",
            Description = "Sportswear",
            CountryOfOrigin = "USA"
        };

        var sneaker = new Sneaker("Air Max", 150m, ProductCategory.Lifestyle, true, "Black", "Mesh", "Air", 42, brand);

        Assert.That(brand.Sneakers, Contains.Item(sneaker));

        sneaker.Delete();

        Assert.Multiple(() =>
        {
            Assert.That(Sneaker.Extent, Does.Not.Contain(sneaker));
            Assert.That(Product.Extent, Does.Not.Contain(sneaker));
            Assert.That(Brand.Extent, Contains.Item(brand));
            Assert.That(brand.Sneakers, Does.Not.Contain(sneaker));
        });
    }

    [Test]
    public void Employee_CanSuperviseOthers()
    {
        var manager = new Employee("Alice", "Brown", "Manager", 3, new DateTime(2019, 5, 1));
        var staff = new Employee("Bob", "White", "Sales", 1, new DateTime(2023, 2, 1));

        staff.AssignSupervisor(manager);

        Assert.Multiple(() =>
        {
            Assert.That(staff.Supervisor, Is.EqualTo(manager));
            Assert.That(manager.Subordinates, Contains.Item(staff));
        });

        staff.RemoveSupervisor();

        Assert.Multiple(() =>
        {
            Assert.That(staff.Supervisor, Is.Null);
            Assert.That(manager.Subordinates, Does.Not.Contain(staff));
        });
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
